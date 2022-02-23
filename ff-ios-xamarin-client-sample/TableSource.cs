using System;
using Foundation;
using UIKit;
using CoreGraphics;

namespace ff_ios_xamarin_client_sample
{

	public class RowArgs : EventArgs
	{
		public string Content { get; set; }
	}
	public class TableSource : UITableViewSource
	{
		const string CellIdentifier = "TableCell";

		public event EventHandler<RowArgs> Selected;
		readonly string[] tableItems;

		public TableSource(string[] items)
		{
			tableItems = items;
		}
		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var handler = Selected;
			if (handler != null)
				handler(this, new RowArgs { Content = tableItems[indexPath.Row] });

			tableView.DeselectRow(indexPath, true);
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return tableItems.Length;
		}


		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);

			cell = cell ?? new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);

			cell.TextLabel.Text = tableItems[indexPath.Row];

			return cell;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return " ";
		}
    }
}
