function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

var GD = {
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
    SelectedNotfall: {},
    SelectedNotfallId :0,
    WartenInfo: false,
    UserName: getCookie("Login"),
    Statistik: {
        Tables: null,
        Columns: null
    },
    Notfalls: {
        Deleted: {
            Id: null,
            Name: null,
            PatientId: null
        }
    }

};

 
