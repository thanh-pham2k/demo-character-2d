using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public sealed class ProductionYearOneExercise
{
    public string question,hint;
    public string[] choices;
    public int answer;
    public YearOneSchedule.Exercise ToExercise()=>new YearOneSchedule.Exercise(question,choices,answer,hint);
    public bool Valid=>!string.IsNullOrWhiteSpace(question) && !string.IsNullOrWhiteSpace(hint) && choices!=null && choices.Length==3 && answer>=0 && answer<3;
}
[Serializable] public sealed class ProductionYearOneCourse
{
    public string id,name;
    public int credits;
    public ProductionYearOneExercise study,exam;
}
[Serializable] public sealed class ProductionYearOneCurriculum
{
    public string scope;
    public ProductionYearOneCourse[] courses;
    public static ProductionYearOneCurriculum Load()
    {
        var asset=Resources.Load<TextAsset>("ProductionCampaign/year-one-courses");
        if(asset==null)throw new InvalidOperationException("Missing year-one curriculum");
        var result=JsonUtility.FromJson<ProductionYearOneCurriculum>(asset.text);
        int credits=0;var ids=new HashSet<string>();
        if(result?.courses==null || result.courses.Length!=6)throw new InvalidOperationException("Year1 needs six authored courses");
        foreach(var course in result.courses)
        {
            if(course==null || string.IsNullOrWhiteSpace(course.id) || !ids.Add(course.id) || string.IsNullOrWhiteSpace(course.name) || course.credits<=0 || course.study==null || !course.study.Valid || course.exam==null || !course.exam.Valid)
                throw new InvalidOperationException("Invalid year-one course");
            credits+=course.credits;
        }
        if(credits!=32)throw new InvalidOperationException("Year-one ledger must total32credits");
        return result;
    }
    public ProductionYearOneCourse ForStep(string step)
    {
        string id=step.StartsWith("study_y1_",StringComparison.Ordinal)?step.Substring(9):step.StartsWith("exam_y1_",StringComparison.Ordinal)?step.Substring(8):null;
        return Array.Find(courses,c=>c.id==id);
    }
}
