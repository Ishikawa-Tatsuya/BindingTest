﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using System.Diagnostics;
using Codeer.Friendly.Windows.Grasp;
using System.Windows.Controls;
using System.Windows;
using System.Linq;

namespace Test
{
    [TestClass]
    public class BindingTest
    {

        WindowsAppFriend _app;
        WindowControl _window;

        [TestInitialize]
        public void TestInitialize()
        {
            _app = new WindowsAppFriend(Process.Start("WpfApplication.exe"));
            _window = WindowControl.FromZTop(_app);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Process.GetProcessById(_app.ProcessId).CloseMainWindow();
        }

        [TestMethod]
        public void TestBindingLogicalTree()
        {
            //指定のオブジェクトを起点とした
            //ロジカルツリー中にあるLhsというパスにバインドされている要素のコレクション
            //Identifyは一つだけであることを保障する
            //コレクションの要素が1以外だった場合は例外発
            _window.LogicalTree().ByBinding("Hoge.Fuga").Identify();
        }

        [TestMethod]
        public void TestBindingVisualTree()
        {
            //VisualTreeもOK
            //この場合2個見つかる
            Assert.AreEqual(2, _window.VisualTree().ByBinding("Hoge.Fuga").Count);

            //タイプ検索も可能
            //この場合なら、組み合わせると特定可能
            _window.VisualTree().ByType(typeof(ContentControl).FullName).Identify().VisualTree().ByBinding("Hoge.Fuga").Identify();
        }

        [TestMethod]
        public void TestBindingDataItem()
        {
            //データアイテムを指定してさらに厳密化することも可能
            ExplicitVar dataItem = new ExplicitVar(_window.Dynamic().DataContext);
            _window.LogicalTree().ByBinding("Hoge.Fuga", dataItem).Identify();
        }

        [TestMethod]
        public void TestBindingAttachedProperty()
        {
            //添付プロパティー
            Assert.AreEqual(1, _window.VisualTree().ByBinding("Piyo").Count);
        }

        [TestMethod]
        public void TestBindingSetter()
        {
            //Setter
            Assert.AreEqual(3, _window.VisualTree().ByBinding("IsSelected").Count);
        }

        [TestMethod]
        public void TestCustom()
        {
            //ユーティリティーを提供するから自分でみつけて
            //普通のWPFプログラムやから
            WindowsAppExpander.LoadAssembly(_app, GetType().Assembly);
            dynamic lbl = _app.Type(GetType()).MyFind(_window, "Color");
            Assert.AreEqual("QQQ", (string)lbl.Content);
        }

        static object MyFind(Window top, string path)
        {
            //Extensionsは良くないから、名前は何とかしよう
            return TreeExtensions.GetVisualTreeCollectionDepthFirst(top).
                Where(e => e.GetType() == typeof(Label)).Cast<Label>().
                Where(e => BindingSearchExtensions.IsMatch(e.Foreground, path)).First();
        }
        
    }
}
