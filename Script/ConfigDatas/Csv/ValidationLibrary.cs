using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ValidationLibraryConfig
{
    #region Configuration 文件中数据段

    /// <summary>问题ID</summary>
    public int QuestionID;
    
    /// <summary>题干</summary>
    public string QuestionStem;

    /// <summary>A选项文字</summary>
    public string AnswerOptionText_1;
    /// <summary>B选项文字</summary>
    public string AnswerOptionText_2;
    /// <summary>C选项文字</summary>
    public string AnswerOptionText_3;

    /// <summary>正确答案</summary>
    public int RightAnswers;
    #endregion

    #region Extend 扩展数据段
    #endregion
}
