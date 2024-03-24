using NUnit.Framework;
using Moq;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ITKarieraAnketi.UIWindows;
using System.Collections.Generic;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.TableItems;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.UIItems.WPFUIItems;
using ITKarieraAnketi.View.UserControls;
using ITKarieraAnketi;
using System.Windows;
using System.Text;
using System.Security.Cryptography;

namespace UnitTest1
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class ClearableTextboxTests
    {
        private ClearableTextbox? clearableTextbox;

        [SetUp]
        public void SetUp()
        {
            clearableTextbox = new ClearableTextbox();
        }

        [Test]
        public void TestTextProperty()
        {
            
            string expectedText = "Test text";
            clearableTextbox.Text = expectedText;
            Assert.That(clearableTextbox.Text, Is.EqualTo(expectedText));
        }
        [Test]
        public void TestClearableTextboxConstructor()
        {
            Assert.That(clearableTextbox.Text, Is.Empty);
            Assert.That(clearableTextbox.TBplaceholder, Is.Null);
        }

        [Test]
        public void TestButtonClearClick()
        {
            clearableTextbox.Text = "Test text";
            clearableTextbox.buttonClear_Click(null, null);
            Assert.That(clearableTextbox.Text, Is.Empty);
        }

        [Test]
        public void TestTBplaceholderProperty()
        {
            string expectedPlaceholder = "Test placeholder";
            clearableTextbox.TBplaceholder = expectedPlaceholder;
            Assert.That(clearableTextbox.TBplaceholder, Is.EqualTo(expectedPlaceholder));
        }

        
    }
    [TestFixture, Apartment(ApartmentState.STA)]
    public class SurveyCreationWindowTests
    {
        private SurveyCreationWindow? surveyCreationWindow;

        [SetUp]
        public void SetUp()
        {
            surveyCreationWindow = new SurveyCreationWindow("Test survey");
        }

        [Test]
        public void TestGetQuestionFromInputs()
        {
            string expectedQuestionText = "Test question";
            string[] expectedAnswerTexts = { "Test answer 1", "Test answer 2" };

            surveyCreationWindow?.SetQuestionTitle(expectedQuestionText);
            for (int i = 0; i < expectedAnswerTexts.Length; i++)
            {
                surveyCreationWindow?.SetAnswerBoxText(i, expectedAnswerTexts[i]);
            }

            var question = surveyCreationWindow?.GetQuestionFromInputs();

            if (question != null)
            {
                Assert.That(question.QuestionText, Is.EqualTo(expectedQuestionText));
                for (int i = 0; i < expectedAnswerTexts.Length; i++)
                {
                    Assert.That(question.Answers[i].AnswerText, Is.EqualTo(expectedAnswerTexts[i]));
                }
            }
        }
        [Test]
        public void TestConstructor()
        {
            string expectedSurveyName = "Test survey";
            var window = new SurveyCreationWindow(expectedSurveyName);
            Assert.That(window.surveyName, Is.EqualTo(expectedSurveyName));
        }
        [Test]
        public void TestAddAnswerBox()
        {
            var window = new SurveyCreationWindow("Test survey");
            int initialAnswerBoxCount = window.answerBoxes.Count;
            window.AddAnswerBox(null, null);
            Assert.That(window.answerBoxes.Count, Is.EqualTo(initialAnswerBoxCount + 1));
        }
        [Test]
        public void TestRemoveAnswerBox()
        {
            var window = new SurveyCreationWindow("Test survey");
            window.AddAnswerBox(null, null);
            int initialAnswerBoxCount = window.answerBoxes.Count;
            window.RemoveAnswerBox(null, null);
            Assert.That(window.answerBoxes.Count, Is.EqualTo(initialAnswerBoxCount - 1));
        }
        [Test]
        public void TestGoToNextQuestion()
        {
            surveyCreationWindow?.SetQuestionTitle("Question 1");
            surveyCreationWindow?.SetAnswerBoxText(0, "Answer 1");
            surveyCreationWindow?.SetAnswerBoxText(1, "Answer 2");
            surveyCreationWindow?.GoToNextQuestion();

            Assert.That(surveyCreationWindow?.CurrentQuestionIndex, Is.EqualTo(1));
            Assert.That(surveyCreationWindow?.QuestionTitleTextBox.Text, Is.EqualTo(string.Empty));
            Assert.That(surveyCreationWindow?.GetAnswerBoxText(0), Is.EqualTo(string.Empty));
            Assert.That(surveyCreationWindow?.GetAnswerBoxText(1), Is.EqualTo(string.Empty));
        }

        [TestFixture, Apartment(ApartmentState.STA)]
        public class InputDialogTests
        {
            private InputDialog? inputDialog;

            [SetUp]
            public void SetUp()
            {
                inputDialog = new InputDialog("Test question");
            }

            [Test]
            public void TestResponseTextProperty()
            {
                string expectedText = "Test response";
                inputDialog.ResponseText = expectedText;
                Assert.That(inputDialog.ResponseText, Is.EqualTo(expectedText));
            }

            [Test]
            public void TestTryCloseDialog()
            {
                inputDialog.ResponseText = "Test response";
                bool result = inputDialog.TryCloseDialog();
                Assert.That(result, Is.False);
            }
        }
        [TestFixture]
        public class LoginWindowTests
        {
            private LoginWindow? loginWindow;

            [SetUp]
            public void SetUp()
            {
                loginWindow = new LoginWindow();
            }

            [Test]
            public void TestHashUserPassword()
            {
                string password = "TestPassword";
                string expectedHash;

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    expectedHash = builder.ToString();
                }
                string actualHash = loginWindow.HashUserPassword(password);
                Assert.That(actualHash, Is.EqualTo(expectedHash));
            }
        }

    }              
}
