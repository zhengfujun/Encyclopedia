using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestionConfig
{
    #region Configuration 文件中数据段

    /// <summary>问题ID</summary>
    public int QuestionID;

    /// <summary>知识体系(同ChapterID)</summary>
    public int SystemID;

    /// <summary>隶属题库类型（0:外圈普通问答格子使用的问题；1:Boss使用的专用问题）</summary>
    public int LibraryType;

    /// <summary>题干</summary>
    public string QuestionStem;

    /// <summary>A选项文字</summary>
    public string AnswerOptionText_A;
    /// <summary>B选项文字</summary>
    public string AnswerOptionText_B;
    /// <summary>C选项文字</summary>
    public string AnswerOptionText_C;
    /// <summary>D选项文字</summary>
    public string AnswerOptionText_D;

    /// <summary>A选项配图</summary>
    public string AnswerOptionPic_A;
    /// <summary>B选项配图</summary>
    public string AnswerOptionPic_B;
    /// <summary>C选项配图</summary>
    public string AnswerOptionPic_C;
    /// <summary>D选项配图</summary>
    public string AnswerOptionPic_D;

    /// <summary>正确答案</summary>
    public string RightAnswers;

    /// <summary>是非题</summary>
    public int YesOrNo;

    /// <summary>语音</summary>
    public string Voice;

    /// <summary>答错后语音</summary>
    public string WrongVoice;
    #endregion

    #region Extend 扩展数据段
    #endregion
}
