using Overust.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ITK.Extensions;

namespace Overust.Views
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl
    {
        public ChatViewModel ViewModel {get; set;}

        public ChatView()
        {
            ViewModel = new ChatViewModel();


            App.RustRcon.ChatLogUpdated += (s, args) =>
                {
                    if (ViewModel.ChatSettings.IsAutoScrollEnabled)
                        ChatScrollViewer.ScrollToBottom();
                };

            App.RustRcon.ChatLogUpdated += (s, args) =>
                {
                    string message = args.Message;
                    //Console.WriteLine("Message: {0}", message);
                    if(ViewModel.ChatSettings.IsTimestampingEnabled)
                        message = args.Timestamp.ToString("(hh:mm tt) ") + message;

                    //var p = new Paragraph();
                    //var part1 = new Run(message.Substring(0, message.Length / 2));
                    //part1.Background = Brushes.Black;
                    //part1.Foreground = Brushes.White;

                    //var part2 = new Run(message.Substring(message.Length / 2, (message.Length / 2) - 1));
                    //p.Inlines.Add(part1);
                    //p.Inlines.Add(part2);

                    //ChatTextBox.Document.Blocks.Add(p);

                    //ChatTextBox.AppendText(message);
                    //ChatTextBox.
                    //if(message.Contains("admin"))
                    //{
                    //    //var start = ChatTextBox.Document.ContentEnd.GetLineStartPosition(0);
                    //    //var end= ChatTextBox.Document.ContentEnd;

                    //    //TextRange range = new TextRange(start, end);
                    //    //range.ApplyPropertyValue(RichTextBox.ForegroundProperty, Brushes.Red);
                    //    //ChatTextBox.Document.Blocks.LastBlock.Foreground = Brushes.Red;
                    //}

                    //ChatTextBox.AppendText(Environment.NewLine);
                        //ChatTextBox.Document.Blocks.LastBlock.Foreground = Brushes.Black;

                    var run = new Run(message);
                    if(ViewModel.ChatSettings.IsNotificationsEnabled)
                    {
                        foreach(var notificationWord in ViewModel.ChatSettings.NotificationWords)
                        {
                            if (message.Contains(notificationWord))
                            {
                                run.Foreground = new SolidColorBrush(Color.FromRgb(68, 138, 255));

                                if(ViewModel.ChatSettings.IsFlashWindowNotificationEnabled)
                                {
                                    Application.Current.MainWindow.FlashWindow();
                                }
                                break;
                            }
                        }
                    }

                    // TODO: Fix this when garry adds steamids to chat messages
                    //if(ViewModel.AutoModerationSettings.IsChatModerationEnabled)
                    //{
                    //    foreach(var kickWord in ViewModel.AutoModerationSettings.ChatKickModerationWords)
                    //    {
                    //        if(message.Contains(kickWord))
                    //        {
                    //            App.RustRcon.KickPlayer(
                    //        }
                    //    }
                    //}

                    ChatTextBox.Document.Blocks.Add(new Paragraph(run));

                    //Chat += message;
                };
            InitializeComponent();
        }

        private void ChatBoxKeyDown(object sender, KeyEventArgs e)
        {
            var textbox = sender as TextBox;

            if(textbox != null)
            {
                if(e.Key == Key.Enter)
                {
                    if(ViewModel.ChatSettings.IsCustomServerConsoleNameEnabled)
                        App.RustRcon.Say(textbox.Text, ViewModel.ChatSettings.CustomServerConsoleName);
                    else
                        App.RustRcon.Say(textbox.Text);

                    textbox.Clear();
                }
            }
        }

        private void ClearChatButtonClick(object sender, RoutedEventArgs e)
        {
            ChatTextBox.Document.Blocks.Clear();
        }
    }
}
