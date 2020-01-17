// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.


using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System.Collections.ObjectModel;
using System.Drawing;

namespace CalculatorUITestFramework
{
    public class MemoryPanel
    {
        private WindowsDriver<WindowsElement> session => WinAppDriver.Instance.CalculatorSession;
        public CalculatorApp CalculatorApp = new CalculatorApp();
        public WindowsElement MemoryClear => this.session.TryFindElementByAccessibilityId("ClearMemoryButton");
        public WindowsElement MemRecall => this.session.TryFindElementByAccessibilityId("MemRecall");
        public WindowsElement MemPlus => this.session.TryFindElementByAccessibilityId("MemPlus");
        public WindowsElement MemMinus => this.session.TryFindElementByAccessibilityId("MemMinus");
        public WindowsElement MemButton => this.session.TryFindElementByAccessibilityId("memButton");
        public WindowsElement MemoryButton => this.session.TryFindElementByAccessibilityId("MemoryButton");
        private WindowsElement MemoryPane => this.session.TryFindElementByAccessibilityId("MemoryPanel");
        private WindowsElement MemoryLabel => this.session.TryFindElementByAccessibilityId("MemoryLabel");
        private WindowsElement MemoryListView => this.session.TryFindElementByAccessibilityId("MemoryListView");
        private WindowsElement MemoryPaneEmptyLabel => this.session.TryFindElementByAccessibilityId("MemoryPaneEmpty");
        public WindowsElement ClearMemoryItemButton => this.session.TryFindElementByAccessibilityId("MClearButton");
        public WindowsElement MemMinusItem => this.session.TryFindElementByAccessibilityId("MSubButton");
        public WindowsElement MemPlusItem => this.session.TryFindElementByAccessibilityId("MAddButton");
        public WindowsElement ClearMemory => this.session.TryFindElementByAccessibilityId("ClearMemory");
        public WindowsElement ListViewItem => this.session.FindElementByClassName("ListViewItem");
        private WindowsElement MemoryFlyout => this.session.TryFindElementByAccessibilityId("MemoryFlyout");

        /// <summary>
        /// Opens the Memory Pane by clicking the Memory pivot label.
        /// </summary>
        public void OpenMemoryPanel()
        {
            this.ResizeWindowToDiplayMemoryLabel();
            this.MemoryLabel.Click();
            this.MemoryPane.WaitForDisplayed();
        }
        /// <summary>
        /// Gets all of the memory items listed in the Memory Pane.
        /// </summary>
        /// <returns>A readonly collection of memory items.</returns>
        public ReadOnlyCollection<AppiumWebElement> GetAllMemoryListViewItems()
        {
            OpenMemoryPanel();
            return this.MemoryListView.FindElementsByClassName("ListViewItem");
        }
        /// <summary>
        /// Opens the Memory Panel and clicks the delete button if it is visible
        /// </summary>
        public void ClearMemoryPanel()
        {
            this.MemoryLabel.Click();

            try
            {
                if (this.session.PageSource.Contains("ClearMemoryButton"))
                {
                    this.ClearMemory.Click();
                }
            }
            catch (WebDriverException ex)
            {
                if (ex.Message.Contains("element could not be located"))
                {
                    Assert.IsNotNull(this.MemoryPaneEmptyLabel);
                    return;
                }

                throw;
            }
        }
        /// <summary>
        /// If the Memory label is not displayed, resize the window
        /// Two attempts are made, the the lable is not found a "not found" exception is thrown
        /// </summary>
        public void ResizeWindowToDiplayMemoryLabel()
        {
            WinAppDriver.Instance.CalculatorSession.Manage().Window.Position = new Point(8, 8);
            if (this.session.PageSource.Contains("MemoryLabel"))
            {
                return;
            }
            else
            {
                WinAppDriver.Instance.CalculatorSession.Manage().Window.Size = new Size(1200, 1050);
                if (this.session.PageSource.Contains("MemoryLabel"))
                {
                    return;
                }
                else
                {
                    WinAppDriver.Instance.CalculatorSession.Manage().Window.Size = new Size(2097, 1282);
                }
                if (this.session.PageSource.Contains("MemoryLabel"))
                {
                    return;
                }
                else
                {
                    throw new NotFoundException("Could not the Memory Label");
                }
            }
        }
        /// <summary>
        /// If the Memory button is not displayed, resize the window
        /// </summary>
        public void ResizeWindowToDiplayMemoryButton()
        {
            WinAppDriver.Instance.CalculatorSession.Manage().Window.Position = new Point(8, 8);
            string source = this.session.PageSource;
            if (this.session.PageSource.Contains("Alt, M"))
            {
                return;
            }
            else
            {
                //this.MemButton.Click();
                source = this.session.PageSource;
                if (this.session.PageSource.Contains("Alt, M"))
                {
                    return;
                }
                else
                {
                    WinAppDriver.Instance.CalculatorSession.Manage().Window.Size = new Size(464, 464);
                    source = this.session.PageSource;
                    if (source.Contains("Alt, M"))
                    {
                        return;
                    }
                    else
                    {
                        throw new NotFoundException("Could not the History Button");
                    }
                }
            }
        }
        /// <summary>
        /// Opens the Memory Flyout.
        /// </summary>
        public void OpenMemoryFlyout()
        {
            this.ResizeWindowToDiplayMemoryButton();
            this.CalculatorApp.EnsureCalculatorHasFocus();
            Actions moveToMemoryButton = new Actions(WinAppDriver.Instance.CalculatorSession);
            moveToMemoryButton.MoveToElement(MemoryButton);
            moveToMemoryButton.Perform();
            this.CalculatorApp.Header.SendKeys(Keys.Alt + "m" + Keys.Alt);
            Actions moveToMemoryFlyout = new Actions(WinAppDriver.Instance.CalculatorSession);
            moveToMemoryFlyout.MoveToElement(MemoryFlyout);
            moveToMemoryFlyout.Perform();
        }
        /// <summary>
        /// Gets all of the memory items listed in the Memory Flyout.
        /// </summary>
        /// <returns> A read only collection of memory items.</returns>
        public ReadOnlyCollection<AppiumWebElement> GetAllMemoryFlyoutListViewItems()
        {
            OpenMemoryFlyout();
            return this.MemoryListView.FindElementsByClassName("ListViewItem");
        }
    }
}
