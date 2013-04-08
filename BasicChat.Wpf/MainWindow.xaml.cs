using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace BasicChat.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public System.Threading.Thread Thread { get; set; }
        public string Host = "http://localhost:8081/";   // "http://localhost:8081/" | "http://localhost:44914/"

        public IHubProxy ProxyInvoke { get; set; }
        public HubConnection ConnectionInvoke { get; set; }

        public bool Active { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ActionSendButtonClick(object sender, RoutedEventArgs e)
        {
            await SendMessage();
        }

        private async Task SendMessage()
        {
            await ProxyInvoke.Invoke("send", ClientNameTextBox.Text + ": " + MessageTextBox.Text);
        }

        private async void ActionWindowLoaded(object sender, RoutedEventArgs e)
        {
            Active = true;
            Thread = new System.Threading.Thread(() =>
            {
                var connectionOn = new HubConnection(Host);
                IHubProxy chatOn = connectionOn.CreateHubProxy("Chat");

                chatOn.On<string>("send", OnSendData);

                connectionOn.Start();

                while (Active)
                {
                    System.Threading.Thread.Sleep(10);
                }
            }) { IsBackground = true };
            Thread.Start();

            ConnectionInvoke = new HubConnection(Host);
            ProxyInvoke = ConnectionInvoke.CreateHubProxy("Chat");
            await ConnectionInvoke.Start();
        }

        private void OnSendData(string message)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => MessagesListBox.Items.Insert(0, message))); 
        }

        private async void ActionMessageTextBoxOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                await SendMessage();
                MessageTextBox.Text = "";
            }
        }
    }
}
