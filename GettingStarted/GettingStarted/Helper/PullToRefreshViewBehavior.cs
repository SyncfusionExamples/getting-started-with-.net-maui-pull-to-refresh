using Syncfusion.Maui.ListView;
using Syncfusion.Maui.PullToRefresh;

namespace GettingStarted;

/// <summary>
/// Base generic class for user-defined behaviors that can respond to conditions and events.
/// </summary>
public class PullToRefreshViewBehavior : Behavior<ContentPage>
{
    private SfListView? listView;
    private SfListView? windListView;
    private Syncfusion.Maui.PullToRefresh.SfPullToRefresh? pullToRefresh;
    private PullToRefreshViewModel? viewModel;

    /// <summary>
    /// Used to Update weather data value
    /// </summary>
    internal void UpdateData()
    {
        var weatherType = this.viewModel!.Data!.WeatherType;
        this.viewModel.Data.Temperature = this.viewModel.UpdateTemperature(weatherType!);
    }

    /// <summary>
    /// You can override this method while View was detached from window
    /// </summary>
    /// <param name="bindable">Sample View typed parameter named as bindable</param>
    protected override void OnAttachedTo(ContentPage bindable)
    {
        this.viewModel = new PullToRefreshViewModel();
        bindable.BindingContext = this.viewModel;
        this.pullToRefresh = bindable.FindByName<Syncfusion.Maui.PullToRefresh.SfPullToRefresh>("pullToRefresh");
        this.listView = bindable.FindByName<SfListView>("listView");
        this.windListView = bindable.FindByName<SfListView>("windList");
        this.pullToRefresh.PullingThreshold = 100;
        this.listView.BindingContext = this.viewModel;
        this.listView.ItemsSource = this.viewModel.SelectedData;
        this.listView.SelectedItem = (this.listView.BindingContext as PullToRefreshViewModel)?.SelectedData![0];
        this.pullToRefresh.Refreshing += this.PullToRefresh_Refreshing;
        this.listView.SelectionChanging += this.ListView_SelectionChanging;
        base.OnAttachedTo(bindable);
    }

    /// <summary>
    /// You can override this method while View was detached from window
    /// </summary>
    /// <param name="bindAble">SampleView typed parameter named as bindAble</param>
    protected override void OnDetachingFrom(ContentPage bindAble)
    {
        this.pullToRefresh!.Refreshing -= this.PullToRefresh_Refreshing;
        this.listView!.SelectionChanging -= this.ListView_SelectionChanging;
        this.pullToRefresh = null;
        this.viewModel = null;
        this.listView = null;
        base.OnDetachingFrom(bindAble);
    }

    /// <summary>
    /// Triggers while pulltorefresh view was refreshing
    /// </summary>
    /// <param name="sender">PullToRefresh_Refreshing sender</param>
    /// <param name="args">PullToRefresh_Refreshing event args e</param>
    private void PullToRefresh_Refreshing(object? sender, EventArgs args)
    {
        this.pullToRefresh!.IsRefreshing = true;
        Dispatcher.StartTimer(
            new TimeSpan( 0, 0, 0, 1, 3000), () =>
            {
                this.UpdateData();
                this.pullToRefresh.IsRefreshing = false;
                return false;
            });
    }

    /// <summary>
    /// Triggers while list view selection was changing
    /// </summary>
    /// <param name="sender">ListView_SelectionChanging event sender</param>
    /// <param name="e">ListView_SelectionChanging event args e</param>
    private void ListView_SelectionChanging(object? sender, ItemSelectionChangingEventArgs e)
    {
        if (e.AddedItems!.Count > 0)
        {
            this.viewModel!.Data = (WeatherData)e.AddedItems[0];
        }
        else if (e.AddedItems.Count == 0)
        {
            e.Cancel = true;
        }
    }
}