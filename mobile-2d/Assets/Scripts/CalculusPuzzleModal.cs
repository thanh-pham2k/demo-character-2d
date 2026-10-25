using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Reusable easy course questions. No story state or movement lives here.
public class CalculusPuzzleModal : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text timerText, feedback;
    [SerializeField] private Button[] answers;
    [SerializeField] private Button retry;
    [SerializeField] private TMP_Text questionText,headingText;
    private int answerIndex=CorrectAnswer;
    private string hint="Gợi ý: cộng số đầu và số cuối thành từng cặp.";
    public int ActiveCorrectAnswer=>answerIndex;
    public void ConfigureLabels(TMP_Text heading,TMP_Text question){headingText=heading;questionText=question;}
    public void SetCourse(YearOneSchedule.Course course,YearOneSchedule.Work work)
    {
        if(course==null)throw new ArgumentNullException(nameof(course));
        string label=work==YearOneSchedule.Work.Homework?"BÀI TẬP":work==YearOneSchedule.Work.Revision?"ÔN THI":work==YearOneSchedule.Work.Exam?"KIỂM TRA":"BÀI HỌC";
        SetExercise(course.Name+" • "+label,course.ExerciseFor(work));
    }
    public void SetExercise(string heading,YearOneSchedule.Exercise exercise)
    {
        if(exercise==null || exercise.Choices.Length!=answers.Length)throw new ArgumentException("Question must match answer buttons");
        if(headingText!=null)headingText.text=heading;
        if(questionText!=null)questionText.text=exercise.Question;
        answerIndex=exercise.Answer;hint=exercise.Hint;
        for(int i=0;i<answers.Length;i++)answers[i].GetComponentInChildren<TMP_Text>().text="["+(i+1)+"] "+exercise.Choices[i];
    }
    private Action onSolved;
    public bool IsOpen => panel != null && panel.activeSelf;
    public bool AwaitingRetry { get; private set; }
    public float SecondsRemaining { get; private set; }
    public int Attempts { get; private set; }
    public const int CorrectAnswer = 1;

    public void Configure(GameObject root, TMP_Text timer, TMP_Text message, Button[] choices, Button retryButton)
    { panel=root; timerText=timer; feedback=message; answers=choices; retry=retryButton; }
    private void Awake()
    {
        for (int i=0;i<answers.Length;i++) { int choice=i; answers[i].onClick.AddListener(()=>Choose(choice)); }
        retry.onClick.AddListener(Retry);
        panel.SetActive(false);
    }
    public void Open(Action solved)
    { onSolved=solved; Attempts=0; panel.SetActive(true); Retry(); }
    public void Cancel(){onSolved=null;if(panel!=null)panel.SetActive(false);}
    public void Retry()
    {
        if(!IsOpen)return;
        Attempts++; SecondsRemaining=20f; AwaitingRetry=false;
        feedback.text=hint;
        foreach(var answer in answers)answer.interactable=true;
        retry.gameObject.SetActive(false);
    }
    public void Choose(int index)
    {
        if(!IsOpen || AwaitingRetry || index<0 || index>=answers.Length)return;
        if(index!=answerIndex) { Fail("Chưa đúng. "+hint); return; }
        panel.SetActive(false);
        var callback=onSolved; onSolved=null; callback?.Invoke();
    }
    private void Fail(string message)
    {
        AwaitingRetry=true; feedback.text=message;
        foreach(var answer in answers)answer.interactable=false;
        retry.gameObject.SetActive(true);
    }
    private void Update()
    {
        if(!IsOpen)return;
        if(!AwaitingRetry)
        {
            SecondsRemaining=Mathf.Max(0,SecondsRemaining-Time.deltaTime);
            timerText.text="Thời gian: "+Mathf.CeilToInt(SecondsRemaining)+"s";
            if(SecondsRemaining<=0)Fail("Hết giờ! "+hint+" Bấm Thử lại để tiếp tục.");
        }
        var keys=Keyboard.current;if(keys==null)return;
        if(AwaitingRetry){if(keys.rKey.wasPressedThisFrame)Retry();return;}
        if(keys.digit1Key.wasPressedThisFrame)Choose(0);
        else if(keys.digit2Key.wasPressedThisFrame)Choose(1);
        else if(keys.digit3Key.wasPressedThisFrame)Choose(2);
    }
    private void OnDisable(){if(panel!=null)panel.SetActive(false);onSolved=null;}
}
