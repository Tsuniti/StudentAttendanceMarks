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

        //private void Button_Click(object sender, RoutedEventArgs e) //For testing
        //{
        //    var db = new StudentAttendanceMarksDB();

        //    var student = new Student();

        //    student.FullName = "Серега Пират";

        //    string imagePath = "C:\\Users\\Tsuni\\Pictures\\Saved Pictures\\Пират.jpg";

        //    // Чтение изображения в байтовый массив
        //    byte[] imageBytes;
        //    using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            fs.CopyTo(ms);
        //            imageBytes = ms.ToArray();
        //        }
        //    }

        //    student.Photo = imageBytes;

        //    student.LastOnline = DateTime.Now;

        //    db.Add(student);
        //    db.SaveChanges();

        //    var button = sender as Button;
        //    button.Content = "Completed";
        //}


        //private void Button_Click(object sender, RoutedEventArgs e) //For testing
        //{
        //    var db = new StudentAttendanceMarksDB();

        //    var lesson = new Lesson();

        //    //lesson.Name = "Розробка додатків з використанням WPF 4.0.0.";

        //    //lesson.DayAndTime = new DateTime(2024, 05, 25, 9, 0, 0);

        //    //db.Add(lesson);


        //    //lesson.Name = "Розробка додатків з використанням WPF 4.0.0.";

        //    //lesson.DayAndTime = new DateTime(2024, 05, 25, 10, 30, 0);

        //    //db.Add(lesson);


        //    //lesson.Name = "Курсовой проект по .NET Framework";

        //    //lesson.DayAndTime = new DateTime(2024, 05, 25, 12, 10, 0);

        //    //db.Add(lesson);


        //    //lesson.Name = "Программирование и администрирование СУБД MS SQL Server";

        //    //lesson.DayAndTime = new DateTime(2024, 05, 25, 9, 0, 0);

        //    //db.Add(lesson);


        //    //lesson.Name = "Программирование и администрирование СУБД MS SQL Server";

        //    //lesson.DayAndTime = new DateTime(2024, 05, 25, 10, 30, 0);

        //    //db.Add(lesson);


        //    //lesson.Name = "Программирование и администрирование СУБД MS SQL Server";

        //    //lesson.DayAndTime = new DateTime(2024, 05, 25, 12, 10, 0);

        //    //db.Add(lesson);


        //    //db.SaveChanges();
        //    //var button = sender as Button;
        //    //button.Content = "Completed";
        //}

        //private void Button_Click(object sender, RoutedEventArgs e) //For testing
        //{
        //    var db = new StudentAttendanceMarksDB();


        //    foreach (var student in db.GetAllStudents())
        //    {

        //        var marks = new Marks();

        //        marks.LessonId = Guid.Parse("A23011D1-5684-47FE-A038-7F00F8A60A1F");
        //        marks.StudentId = student.Id;

        //        db.Add(marks);
        //        db.SaveChanges();
        //    }

        //    var button = sender as Button;
        //    button.Content = "Completed";
        //}
    }
}