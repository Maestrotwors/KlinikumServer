var GM = {
    PatientenClick: function (id) {
        //router.go("/patient/" + id);
    },
    NewPatient: function () {
        //router.go("/patient/new");
    },
    NewNotfall: function () {
        //router.go('/notfall/new');
    },
    NotfallChanged: function (rout) {
        this.NotfallSelect(rout.query.id, rout);
        //NotfallChanged(this.$route.params.NotfallId);
    },
    Notfall_tab_change: function (tab, rout) {
        var id_ = 0;
        //console.log(rout);
        try { id_ = rout.query.id } catch (e) { }
        router.push({ query: { id: id_, tab: tab } }); 
    },
    Statistik_Tab_change: function (TabId) {
        router.push({ query: { tab: TabId} })
    },
    SaveParam: function (AnswerId, AnswerQuestion, ThisValue) {
        //console.log(AnswerId + "---" + AnswerQuestion + "----" + ThisValue);
        $.ajax({
            url: '/api/save_param_value',
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            type: 'POST', data: { "NotfallId": GD.SelectedNotfallId, "AnswerId": AnswerId, "ThisValue": ThisValue },
            success: function (data) {
                //router.push({ name: 'NotfallPage', params: { "NotfallId": 36 } })
                //GD.SelectedNotfall = JSON.parse(data)[0];
            }
        });
    },
    NotfallSelect: function (NotfallId, rout) { 
        if (NotfallId == undefined) { return }
        var tab = 1;
        try { tab = rout.query.tab; } finally{if (tab == undefined) { tab = 1; } } 
        GD.Model_Quest = {};
        GD.SelectedNotfallId = NotfallId;
        GD.WartenInfo = true;
        $.ajax({
            url: '/api/get_notfall_data',
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            type: 'POST', data: { "NotfallId": NotfallId },
            success: function (data) {
                //console.log(NotfallId + "---" + tab);
                router.push({query: { id: NotfallId, tab: tab} });
                GD.SelectedNotfall = JSON.parse(data)[0];
            }
        });
        $.ajax({
            url: '/api/get_notfall_values',
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            type: 'POST', data: { "NotfallId": NotfallId },
            success: function (data) {
                arr = JSON.parse(data);
                GD.Model_Quest = {};
                arr.forEach(function (item, i, arr) {
                    //console.log(item.AnswerId);
                    GD.Model_Quest[item.AnswerId] = item.Value;
                });
                GD.WartenInfo = false;
            }
        });

    },
    GetUndefinedBool: function (rout, query_param, param2) {
        try { if (rout.query[query_param] == param2) { return true; } } catch(e) { return false };
    },
    GoToNotfalls: function () {
        router.push({ name: 'NotfallePage', params: {} })
    },
    CreateNotfall: function () {
        $.ajax({
            url: '/api/new_notfall',
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            type: 'POST', data: { "PatName": GD.NewNotfall_PatientName, "PatVorname": GD.NewNotfall_PatientVorname, "PatGebDate": GD.NewNotfall_PatientGeburtsdatum, "ReanimationDateTime": "11.11.2011" },
            success: function (data) {
                if (data == "0") { alert("Fehler"); return }
                router.push({ name: 'NotfallPage', params: { "NotfallId": data } });
                GM.GetNotfalls();
            }
        })
    },
    GetNotfalls: function () {
        $.ajax({
            url: '/api/notfalls',
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            success: function (data) {
                GD.NotfallsList = JSON.parse(data);
            }
        });
    },
    GetSofa: function (Param, Value) {
        switch (Param) {
            case 1:
                if (Value > 400) { return 0 }
                else if (Value <= 400 && Value > 300) { return 1 }
                else if (Value <= 300 && Value > 200) { return 2 }
                else if (Value <= 200 && Value > 100) { return 3 }
                else if (Value <= 100) { return 4 }
                else { return "Nicht berechnet" }

            case 2:
                if (Value > 150) { return 0 }
                else if (Value <= 150 && Value > 100) { return 1 }
                else if (Value <= 100 && Value > 50) { return 2 }
                else if (Value <= 50 && Value > 20) { return 3 }
                else if (Value <= 20) { return 4 }
                else { return "Nicht berechnet" }

            case 3:
                if (Value < 1.2) { return 0 }
                else if (Value > 2 && Value <= 1.2) { return 1 }
                else if (Value > 2 && Value <= 6.0) { return 2 }
                else if (Value > 6.0 && Value <= 12) { return 3 }
                else if (Value > 12) { return 4 }
                else { return "Nicht berechnet" }

            case 4:
                if (GD.Model_Quest[67] > 0) { return 4 };
                if (GD.Model_Quest[65] > 0) { return 3 };
                if (GD.Model_Quest[31] < 70) { return 1 };
                return 0;
            default:
            //alert('Nicht berechnet');
        }
    },
    getSofaSumm: function () {
        x = parseInt(this.GetSofa(1, Model_Quest[129] / Model_Quest[131])) + parseInt(this.GetSofa(2, Model_Quest[71])) + parseInt(this.GetSofa(3, Model_Quest[80]));
        if (isNaN(x)) { return "Nicht alle Daten sind berechnet" }
        else { return x; }
    },
    getCriteriaSepsis: function () {
        var CriteriaSepsisArray = [GD.Model_Quest[12], "Апельсин", "Слива"];
        return CriteriaSepsisArray;
    },
    table_art_hypoxemia: function () {
        //x = GD.Model_Quest[129]/GD.Model_Quest[131];
        //alert(x);
        //if (x < 300){return true}else{return false}
        return true;
    },
    BtoInt: function (x) { if (x == true || x == "true" || x == 1 || x == "1") { return 1; } else return 0; }

};

SaveParam = function (AnswerId, AnswerQuestion, ThisValue) {
    //alert(ThisValue);
    GM.SaveParam(AnswerId, AnswerQuestion, ThisValue); 
}