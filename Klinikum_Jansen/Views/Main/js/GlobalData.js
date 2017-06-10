function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

var GD = {
    altePass:"", neuePass1:"", neuePass2:"",
    PatientenList: [],
    NotfallsList: [],
    NotfallsDataMap: [],
    AnswersList: [],
    AnswersModel: {},
    QuestionsModel: {}, Questions: [], Answers: [],
    Notfall_search: "",
    NewNotfall_PatientGender: null,
    NewNotfall_PatientReanimationTime: "",
    NewNotfall_PatientReanimationDate: "",
    NewNotfall_PatientGeburtsdatum: "",
    NewNotfall_PatientVorname: "",
    NewNotfall_PatientName: "",
    Notfall_Quest: [],
    Model_Quest: [],
    SelectedNotfall: {},//OLD
    SelectedNotfallId: 0,//OLD
    WartenInfo: false,
    UserName: getCookie("Login"),
    Selected_Notfall: {
        SelectedNotfallId: 0,
        SelectedPatient: { 
        }
    },
    Statistik: {
        Param_Values_Filtered: {},
        Param_Values_Filtered_All: {},
        Param_Values: {},
        Tables: {},
        Columns: {},
        Notfalls: {},
        ShowDescriptionSofa:true,
    },
    Notfalls: {
        Deleted: {
            Id: null,
            Name: null,
            PatientId: null
        }
    }

};

 
