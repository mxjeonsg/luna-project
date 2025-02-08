using System.Data.SQLite;
using System.Threading;
using System.Collections.Concurrent;
using System;

namespace Luna.Framework;

public class SqliteHandler {
    private readonly string connection_string =
        "Data Source=database.db;Version=3;"
    ;
    private readonly Thread dbThread;
    private readonly ConcurrentQueue<Action<SQLiteConnection>> task_queue;
    private bool running = true;

    private void initialiseDatabase() {
        this.executeNonQuery(@"
            create table if not exists Users (
                id integer primary key autoincrement,
                username text unique not null,
                password_hash text not null,
                email text unique not null,
                created_at datetime default CURRENT_TIMESTAMP
            );

            create table if not exists Posts (
                id integer primary key autoincrement,
                user_id integer not null,
                content text not null,
                created_at datetime default CURRENT_TIMESTAMP,
                foreign key (user_id) references Users(id)
            );

            create table if not exists Messages (
                id integer primary key autoincrement,
                sender_id integer not null,
                receiver_id integer not null,
                content text not null,
                sent_at datetime default CURRENT_TIMESTAMP,
                foreign key (sender_id) references Users(id),
                foreign key (receiver_id) references Users(id)
            );

            create table if not exists Interactions (
                id integer primary key autoincrement,
                user_id integer not null,
                post_id integer not null,
                interaction_type text not null,
                created_at datetime default CURRENT_TIMESTAMP,
                foreign key (user_id) references Users(id),
                foreign key (post_id) references Posts(id)
            );
        ");
    }

    private void databaseWorker() {
        using(SQLiteConnection conn = new SQLiteConnection(this.connection_string)) {
            conn.Open();

            while(this.running) {
                if(this.task_queue.TryDequeue(out Action<SQLiteConnection>? task)) {
                    // task!(conn);
                    task(conn);
                } else {
                    Thread.Sleep(10);
                }
            }
        }
    }

    private void executeNonQuery(string query, params SQLiteParameter[] parametres) {
        this.task_queue.Enqueue((conn) => {
            using(SQLiteCommand command = new SQLiteCommand(query, conn)) {
                command.Parameters.AddRange(parametres);
                command.ExecuteNonQuery();
            }
        });
    } 

    public void executeQuery(string query, Action<SQLiteDataReader> handleResults, params SQLiteParameter[] parametres) {
        this.task_queue.Enqueue((conn) => {
            using(SQLiteCommand command = new SQLiteCommand(query, conn)) {
                command.Parameters.AddRange(parametres);

                using(SQLiteDataReader reader = command.ExecuteReader()) {
                    handleResults(reader);
                }
            }
        });
    }

    public void stop() {
        this.running = false;
        this.dbThread.Join();
    }

    public void halt()
    => this.stop();

    public SqliteHandler() {
        this.task_queue = new ConcurrentQueue<Action<SQLiteConnection>>();
        this.dbThread = new Thread(this.databaseWorker) {
            IsBackground = true
        };
        this.dbThread.Start();
        this.initialiseDatabase();
    }

    ~SqliteHandler()
    => this.halt();
}