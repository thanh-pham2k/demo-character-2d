using UnityEngine;

public sealed class ClassroomNpcAnimation : MonoBehaviour
{
    [SerializeField] private YearOneMilestoneController lesson;
    [SerializeField] private SpriteRenderer dam,teacher;
    [SerializeField] private Sprite[] writing,gestures;
    [SerializeField] private Sprite surprised;
    public void Configure(YearOneMilestoneController controller,SpriteRenderer student,SpriteRenderer lecturer,Sprite[] notes,Sprite surprise,Sprite[] lectureFrames)
    {lesson=controller;dam=student;teacher=lecturer;writing=notes;surprised=surprise;gestures=lectureFrames;}
    // Preserve teacher gestures while replacing the former dorm roommate identity.
    public void ConfigureClassmate(Sprite sprite)
    {
        writing = new[]{sprite}; surprised = sprite;
        if(dam!=null)dam.sprite=sprite;
    }
    private void Update()
    {
        if(lesson==null)return;
        bool reaction=lesson.CurrentPhase==YearOneMilestoneController.Phase.Reaction;
        if(dam!=null && writing.Length>0)dam.sprite=reaction && surprised!=null?surprised:writing[(int)(Time.time/.4f)%writing.Length];
        if(teacher!=null && gestures.Length>0)teacher.sprite=gestures[reaction?(int)(Time.time/.5f)%gestures.Length:0];
    }
}
