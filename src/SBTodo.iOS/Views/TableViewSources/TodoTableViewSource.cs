using System;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace SBTodo.iOS.Views.TableViewSources
{
    public class TodoTableViewSource : MvxTableViewSource
    {
        private static readonly NSString TodoCellIdentifier = new NSString("TodoCell");

        public TodoTableViewSource(UITableView tableView) : base(tableView)
        {
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
            tableView.RegisterClassForCellReuse(typeof(TodoTableViewCell),TodoCellIdentifier);
        }

        public TodoTableViewSource(IntPtr handle) : base(handle)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            return (UITableViewCell) TableView.DequeueReusableCell(TodoCellIdentifier, indexPath);
        }
    }
}
