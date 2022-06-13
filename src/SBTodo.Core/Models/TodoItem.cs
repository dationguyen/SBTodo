using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using SQLite;

namespace SBTodo.Core.Models
{
    public class TodoItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Todo { get; set; }
        public bool Completed { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [Ignore]
        public ICommand DeleteCommand { get; set; }

        [Ignore]
        public ICommand EditCommand { get; set; }

        public static TodoItem Create(string message, IAsyncRelayCommand<TodoItem> deleteTodoCommand,  IAsyncRelayCommand<TodoItem> editTodoCommand)
        {
            var todo =  new TodoItem() { Todo = message, DateCreated = DateTime.Now, DateModified = DateTime.Now};
            todo.DeleteCommand = new AsyncRelayCommand(() => deleteTodoCommand.ExecuteAsync(todo));
            todo.EditCommand = new AsyncRelayCommand(() => editTodoCommand.ExecuteAsync(todo));
            return todo;
        }
    }
}
