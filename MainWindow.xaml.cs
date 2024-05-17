using StudentAttendanceMarks.Components;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using StudentAttendanceMarks.Database;

namespace StudentAttendanceMarks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {


        public MainWindow()
        {
            InitializeComponent();
            LessonNamesComboBox.SelectionChanged += LessonNamesComboBox_SelectionChanged;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            using var db = new StudentAttendanceMarksDB();
            HashSet<string> lessonNames = new HashSet<string>();
            foreach (var lesson in db.GetAllLessons())
            {
                if (lessonNames.Add(lesson.Name))
                    LessonNamesComboBox.Items.Add(lesson.Name);
            }

        }

        private void LessonNamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LessonNamesComboBox.SelectedItem is null || LessonNamesComboBox.SelectedItem.ToString() == "-Choose Lesson-") return;
            LessonDaysComboBox.Items.Clear();
            MarksRowsPanel.Children.Clear();

            var defaultComboBoxItem = new ComboBoxItem
            {
                Content = " -Choose day and time- ",
                Tag = "defaultComboBox",
                Visibility = Visibility.Collapsed
            };
            LessonDaysComboBox.Items.Add(defaultComboBoxItem);
            LessonDaysComboBox.SelectedItem = defaultComboBoxItem;
            LessonDaysComboBox.Visibility = Visibility.Visible;

            using var db = new StudentAttendanceMarksDB();
            var selectedLesson = LessonNamesComboBox.SelectedItem.ToString();

            foreach (var lesson in db.GetAllLessons())
            {
                if (lesson.Name == selectedLesson)
                {
                    LessonDaysComboBox.Items.Add(lesson.DayAndTime);
                }
            }
        }

        private void LessonDaysComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LessonDaysComboBox.SelectedItem == null ||
                LessonDaysComboBox.SelectedItem.GetType() != typeof(DateTime)) return;
            
            MarksRowsPanel.Children.Clear();
            using var db = new StudentAttendanceMarksDB();
            var selectedLesson = db.GetAllLessons().FirstOrDefault(l =>
                l.Name == LessonNamesComboBox.SelectedItem.ToString()
                && l.DayAndTime == DateTime.Parse(LessonDaysComboBox.SelectedItem.ToString() ?? string.Empty));

            int i = 0;
            foreach (var marks in db.GetAllMarks())
            {
                if (selectedLesson != null && marks.LessonId == selectedLesson.Id)
                {
                    MarksRowsPanel.Children.Add(new MarksRow(++i, marks));
                }
            }
        }
    }
}