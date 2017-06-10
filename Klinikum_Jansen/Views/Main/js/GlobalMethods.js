var GM = {
        PasswordSpeichern: function (altePass, neuePass1, neuePass2) {
            if (neuePass1!=neuePass2){
                alert("Neues Kennwort ist nicht gleich");
            } else {
                if (altePass == "" || neuePass1 == "" || neuePass2 == "") {
                    alert("Sie haben nicht alles geschrieben.");
                } else {
                    alert("----");
                    $.ajax({
                        url: '/api/passwordChange',
                        headers: {
                            "Content-Type": "application/x-www-form-urlencoded"
                        },
                        type: 'POST', data: { "altePass": altePass, "neuePass": neuePass1},
                        success: function (data) {
                            if (data == "1") { alert("Kennwort wurde geändert"); location.reload(); }
                            if (data == "0") { alert("Password ist falsch");   }
                        }
                    });
                }
            }
        },
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
            router.push({ query: { tab: TabId } });
        },
        Get_Statistik_Tables: function (TabId) {
            return _.where(GD.Statistik.Tables, { TabId: TabId.toString() });
        },
        Get_Statistik_Columns: function (Table_Id) {
            //console.log(_.where(GD.Statistik.Columns, { Table_Id: Table_Id.toString() }));
            return _.where(GD.Statistik.Columns, { Table_Id: Table_Id.toString() });
        },
        /*Get_StatistikTable_Rows: function (Data) {
            //if (Data == "Notfalls") {  
                return GD.Statistik.Notfalls;
            //} 
        },
        Get_ColumnFormule: function (RowId,Column) {
            //console.log(RowId + "---" + Column);
            return Column.Formule;
        },*/
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
        Edit_Patient_SaveParam: function (Param, ThisValue) {  
            if (GD.Selected_Notfall.NotfallId=="0"){return}
            $.ajax({
                url: '/api/edit_patient_info',
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded"
                },
                type: 'POST', data: { "Edit_PatientName": GD.Edit_PatientName, "Edit_PatientVorname": GD.Edit_PatientVorname, "ThisValue": ThisValue },
                success: function (data) {
                    //router.push({ name: 'NotfallPage', params: { "NotfallId": 36 } })
                    //GD.SelectedNotfall = JSON.parse(data)[0];
                }
            });
        },
        Statistik_Table1_Filter: function (param) {
            if (param == "1") {
                GD.Statistik.Param_Values_Filtered = _.filter(GD.Statistik.Param_Values_Filtered_All, function (e) { return e.SofaSumm > 1; });
            }
            if (param == "0") {
                GD.Statistik.Param_Values_Filtered = GD.Statistik.Param_Values_Filtered_All;
            }
        },
        Get_Statistik_Param_Value: function (Notfall_Id, param_Id,AnswerId,compileBool) {
            //console.log(String(Notfall_Id) + "---" + String(param_Id));
            
            //var data = _.where(GD.Statistik.Param_Values, { "NotfallId": id });  
            var returning_Notfall = _.findWhere(GD.Statistik.Param_Values_Filtered, { "NotfallId": Notfall_Id });
            var returning_Param = null;
            if (AnswerId != 0) { returning_Param = _.where(returning_Notfall.Answers, { "QuestionId": String(param_Id), "AnswerId": String(AnswerId) }) } else {
                returning_Param = _.where(returning_Notfall.Answers, { "QuestionId": String(param_Id) });
            }
            //console.log(returning_Param);
            var returning_Value="";
            try {
                //console.log(returning_Param);
                returning_Value = returning_Param[0].Value; 
            } catch (err) {
                returning_Value = 0;
            }

            if (compileBool) { if (returning_Value == "true") { return "Ja"; } else { return "Nein" }}
            return returning_Value;
            //
            //return returning_Value;
            //return GD.Notfalls[Notfall_Id];
        },
        Get_Sofa_Summ: function (NotfallId) {
            return _.findWhere(GD.Statistik.Param_Values_Filtered, { "NotfallId": String(NotfallId) }).SofaSumm;
        },
        Get_NotfallParam: function (NotfallId) {
            return _.findWhere(GD.Statistik.Param_Values_Filtered, { "NotfallId": String(NotfallId) });
        },
        Statistik_Get_Nf_mit_Sofa_mehr_2: function () {
            var arr = {};
            arr = _.filter(GD.Statistik.Param_Values_Filtered, function (e) { return e.SofaSumm > 1; });
            //console.log(arr);
            //console.log("++++++++++");
            return arr.length;
        },
        Statistik_Get_NotfallsCount_SOFA_2: function () {
            return _.findWhere(GD.Statistik.Param_Values_Filtered, { "NotfallId": String(NotfallId) });
        },
        Get_Statistik_Data: function () {
            GD.WartenInfo = true;
            var self = this;

            Get_X_Data = function (X, AnswerId) {
                var data = "-";
                try { data = _.findWhere(X, { "AnswerId": AnswerId }).Value; } catch (ex) {

                } 
                //if (data != undefined) { return data }; return "-";
                return data;
            };

            Get_Data_0= function (value) {
                if (value == "-") { return 0 }
                return value;
            };

            $.ajax({
                url: '/api/get_statistik_data',
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded"
                },
                type: 'POST', data: {},
                success: function (data) {

                    //console.log(NotfallId + "---" + tab);
                    //router.push({ query: { id: NotfallId, tab: tab } });
                    GD.Statistik.Tables = JSON.parse(data)["Tables"];
                    GD.Statistik.Columns = JSON.parse(data)["Columns"];
                    GD.Statistik.Notfalls = JSON.parse(data)["Notfalls"];
                    GD.Statistik.Param_Values = JSON.parse(data)["Param_Values"];
                    //console.log(GD.Statistik.Tables); 
                    GD.Statistik.Param_Values_Filtered = [];
                    GD.Statistik.Param_Values_Filtered_All = [];
                    GD.Statistik.Notfalls.forEach(function (Notfall) {
                        N_Id = String(Notfall.Id);
                        var X = {
                            Answers: {},
                            NotfallId: N_Id,
                            Id: N_Id,
                            Name: Notfall.Name,
                            Vorname: Notfall.Vorname,
                            Sofa: {
                                Param1: null, 
                                Param2: null, 
                                Param3: null, 
                                Param4: null
                            },
                            SofaSumm:0
                        };
                        X.Answers = _.where(GD.Statistik.Param_Values, { "NotfallId": N_Id });
                        X.Sofa.Param_Respiratory = (Get_X_Data(X.Answers, "129") * 750 / Get_X_Data(X.Answers, "131")).toFixed(2);;
                        X.Sofa.Param_Coagulation = Get_X_Data(X.Answers, "71" );
                        X.Sofa.Param_Liver_Bil = Get_X_Data(X.Answers,  "80" );
                        X.Sofa.Param_C_Hyp_67 = Get_X_Data(X.Answers, "67");
                        X.Sofa.Param_C_Hyp_65 = Get_X_Data(X.Answers, "65");
                        X.Sofa.Param_C_Hyp_31 = Get_X_Data(X.Answers, "31");
                        X.Sofa.Param_SOFA_1 = self.GetSofa(1, X.Sofa.Param_Respiratory);
                        X.Sofa.Param_SOFA_2 = self.GetSofa(2, X.Sofa.Param_Coagulation);
                        X.Sofa.Param_SOFA_3 = self.GetSofa(3, X.Sofa.Param_Liver_Bil);
                        X.Sofa.Param_SOFA_4 = self.GetSofa(4, 0, X.Sofa.Param_C_Hyp_67, X.Sofa.Param_C_Hyp_65, X.Sofa.Param_C_Hyp_31);
                        X.SofaSumm = Get_Data_0(X.Sofa.Param_SOFA_1) + Get_Data_0(X.Sofa.Param_SOFA_2) + Get_Data_0(X.Sofa.Param_SOFA_3) + Get_Data_0(X.Sofa.Param_SOFA_4);
                        //console.log(X.Sofa.Param_SOFA_1); 
                        //console.log(X.Sofa.Param_SOFA_2);
                        //console.log(X.Sofa.Param_SOFA_3);
                        //console.log(X.Sofa.Param_SOFA_4);
                        //console.log(X.Sofa.Param_Respiratory);
                        //console.log("--------------");
                        //z = self.Get_Statistik_Param_Value("80", "129");
                        //console.log(z);
                        
                        //console.log("====="+self.Get_Statistik_Param_Value(N_Id, "129"));
                        //X.Sofa.Param1 = self.GetSofa(1, self.Get_Statistik_Param_Value(N_Id, 129) * 750 / self.Get_Statistik_Param_Value(N_Id, 131));
                        

                        //if (X.Sofa.Param1 == "-" || X.Sofa.Param2 == "-" || X.Sofa.Param3 == "-" || X.Sofa.Param4 == "-") {
                        //    X.SofaValue = "---";
                        //} else { X.SofaValue = X.Sofa.Param1 + X.Sofa.Param2 + X.Sofa.Param3 + X.Sofa.Param4 }
                        GD.Statistik.Param_Values_Filtered.push(X);

                        

                        //console.log("param1=" + self.Get_Statistik_Param_Value(Notfall.Id, "129", false));
                    });
                    GD.Statistik.Param_Values_Filtered_All = GD.Statistik.Param_Values_Filtered;
                    GD.WartenInfo = false;


                }
            });
        },
        NotfallSelect: function (NotfallId, rout) {
            if (NotfallId === undefined) { return }
            GD.Selected_Notfall.NotfallId = "0";
            var tab = 1;
            try { tab = rout.query.tab; } finally { if (tab === undefined) { tab = 1; } }
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
                    router.push({ query: { id: NotfallId, tab: tab } }); 
                    //GD.Selected_Notfall.NotfallId = JSON.parse(data)[0].Id
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
            $.ajax({
                url: '/api/get_patient_info',
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded"
                },
                type: 'POST', data: { "NotfallId": NotfallId },
                success: function (data) {
                    arr = JSON.parse(data);
                    console.log(arr);
                    GD.Selected_Notfall.SelectedPatient = arr[0];   
                    GD.WartenInfo = false;
                }
            });

        },
        GetUndefinedBool: function (rout, query_param, param2) {
            try { if (rout.query[query_param] === param2) { return true; } } catch (e) { return false };
        },
        Notfall_Delete: function (NotfallId, PatientId, Notfall_Patient_Name) {
            GD.Notfalls.Deleted.Id = NotfallId;
            //console.log(PatientId);
            GD.Notfalls.Deleted.PatientId = PatientId;
            GD.Notfalls.Deleted.Name = Notfall_Patient_Name;
            $('#Modal_Notfall_Delete').modal();
        },
        Notfall_Delete_Bestatigung: function () {
            $.ajax({
                url: '/api/delete_notfall',
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded"
                },
                type: 'POST', data: { "PatientId": GD.Notfalls.Deleted.PatientId },
                success: function (data) {
                    if (data == "1") {
                        GD.NotfallsList = null;
                        $.ajax({
                            url: '/api/notfalls',
                            headers: {
                                "Content-Type": "application/x-www-form-urlencoded"
                            },
                            success: function (data) {
                                GD.NotfallsList = JSON.parse(data);
                            }
                        });
                    }
                    //console.log(data);
                    //$('#Modal_Notfall_Delete').modal('hide');
                }
            });
            $('#Modal_Notfall_Delete').modal('hide');

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
                type: 'POST', data: { "PatName": GD.NewNotfall_PatientName, "PatVorname": GD.NewNotfall_PatientVorname, "PatGebDate": GD.NewNotfall_PatientGeburtsdatum, "ReanimationDateTime": GD.NewNotfall_PatientReanimationDate + " " + GD.NewNotfall_PatientReanimationTime, "Gender": GD.NewNotfall_PatientGender},
                success: function (data) {
                    if (data === "0") { alert("Fehler"); return }
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
        GetParamValue_Statistik: function () {

        },
        Get_Statistik_Nofall_SOFA: function (Notfall_Id) {
            var returning_Notfall;
            try {
                returning_Value = _.findWhere(GD.Statistik.Param_Values_Filtered, { "NotfallId": String(Notfall_Id) });
                var SofaValue = returning_Value.SofaValue;
                return SofaValue;
            } catch (err) {
                return "---";
            }
        },
        GetSofa: function (Param, Value, Param67, Param65, Param31) {
            switch (Param) {
                case 1:
                    if (Value > 400) { return 0; }
                    if (Value <= 400 && Value > 300) { return 1; }
                    if (Value <= 300 && Value > 200) { return 2; }
                    if (Value <= 200 && Value > 100) { return 3; }
                    if (Value <= 100) { return 4; }
                    else { return "-"; }

                case 2:
                    if (Value > 150) { return 0 }
                    if (Value <= 150 && Value > 100) { return 1; }
                    if (Value <= 100 && Value > 50) { return 2; }
                    if (Value <= 50 && Value > 20) { return 3; }
                    if (Value <= 20) { return 4; }
                    else { return "-"; }

                case 3:
                    if (Value < 1.2) { return 0 }
                    if (Value > 2 && Value <= 1.2) { return 1; }
                    if (Value > 2 && Value <= 6.0) { return 2; }
                    if (Value > 6.0 && Value <= 12) { return 3; }
                    if (Value > 12) { return 4; }
                    else { return "-"; }

                case 4:
                    if (Param67 > 0) { return 4; };
                    if (Param65 > 0) { return 3; };
                    if (Param31 < 70) { return 1; };
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
        //TEST
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
        BtoInt: function (x) { if (x === true || x === "true" || x === 1 || x === "1") { return 1; } else return 0; }

    };

    SaveParam = function (AnswerId, AnswerQuestion, ThisValue) {
        //alert(ThisValue);
        GM.SaveParam(AnswerId, AnswerQuestion, ThisValue);
    }; 


