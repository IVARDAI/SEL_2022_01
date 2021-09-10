
Public Class FP_Map_TomTom
    'Public Property RequestString As String
    'Public Property ResponseMessage As String
    'Public Property ResponseValue As String
    'Public Property Res_Distance_Orig As Integer
    'Public Property Res_Distance_KM As Integer
    'Public Property Res_LngLat As List(Of String)

    Public Property Search_Result_JSON As String
    Public Property Routing_Result_JSON As String
    Public Property ResponseMessage As String

    Public Structure Struct_Request_Params
        Dim SelID As Integer
        Dim Provider As String
        Dim Action As String
        Dim Terminal As String
        Dim UserID As Integer
    End Structure

    Public Structure Struct_Search_Params
        Dim Country As String
        Dim ZIP As String
        Dim City As String
        Dim Addr As String
        Dim PopupTitle As String
        Dim PopupHead As String
        Dim PopupBody As String
    End Structure

    Public Structure Struct_Routing_Params
        Dim ID As Integer
        Dim Location As String
        Dim Weight As Integer
        Dim PopupTitle As String
        Dim PopupHead As String
        Dim PopupBody As String
    End Structure

    Public Function SEARCH(ByVal Search_URL As String, P_Request_Params As Struct_Request_Params, P_Search_Params As Struct_Search_Params, ByVal Map_P As String) As Boolean
        Dim hc As New System.Net.Http.HttpClient
        Dim trm As System.Threading.Tasks.Task(Of System.Net.Http.HttpResponseMessage)
        Dim postparam As New List(Of KeyValuePair(Of String, String))

        Dim request_params As String = """selid"":{0},""provider"":""{1}"",""action"":""{2}"",""terminal"":""{3}"",""userid"":{4}"
        request_params = String.Format(request_params, P_Request_Params.SelID, P_Request_Params.Provider, P_Request_Params.Action, P_Request_Params.Terminal, P_Request_Params.UserID)
        request_params = "{" + request_params + "}"

        Dim function_params As String = """country"":""{0}"",""zip"":""{1}"",""city"":""{2}"",""addr"":""{3}"",""popuptitle"":""{4}"",""popuphead"":""{5}"",""popupbody"":""{6}"""
        function_params = String.Format(function_params, P_Search_Params.Country, P_Search_Params.ZIP, P_Search_Params.City, P_Search_Params.Addr, P_Search_Params.PopupTitle, P_Search_Params.PopupHead, P_Search_Params.PopupBody)
        function_params = "{" + function_params + "}"

        With postparam
            .Add(New KeyValuePair(Of String, String)("request_params", request_params))
            .Add(New KeyValuePair(Of String, String)("function_params", function_params))
            .Add(New KeyValuePair(Of String, String)("map_params", Map_P))
        End With

        Try
            trm = hc.PostAsync(New Uri(Search_URL), New System.Net.Http.FormUrlEncodedContent(postparam))
            Dim rm As System.Net.Http.HttpResponseMessage
            rm = trm.Result
            If rm.IsSuccessStatusCode Then
                Dim ts As System.Threading.Tasks.Task(Of String)
                ts = rm.Content.ReadAsStringAsync
                Dim res As String = ts.Result
                If res <> "" Then
                    Search_Result_JSON = res
                End If
            Else
                SEARCH = False
                Exit Function
            End If

        Catch ex As Exception
            SEARCH = False
            ResponseMessage = ex.Message
            Exit Function
        End Try

        SEARCH = True
    End Function

    Public Function ROUTING(ByVal Routing_URL As String, ByVal P_Request_Params As Struct_Request_Params, ByVal P_Routing_Params As List(Of Struct_Routing_Params), ByVal Map_P As String) As Boolean
        Dim hc As New System.Net.Http.HttpClient
        Dim trm As System.Threading.Tasks.Task(Of System.Net.Http.HttpResponseMessage)
        Dim postparam As New List(Of KeyValuePair(Of String, String))

        Dim request_params As String = """selid"":{0},""provider"":""{1}"",""action"":""{2}"",""terminal"":""{3}"",""userid"":{4}"
        request_params = String.Format(request_params, P_Request_Params.SelID, P_Request_Params.Provider, P_Request_Params.Action, P_Request_Params.Terminal, P_Request_Params.UserID)
        request_params = "{" + request_params + "}"

        Dim function_params As String = ""
        Dim split As String = ""

        For Each p As Struct_Routing_Params In P_Routing_Params
            Dim line As String = """id"":""{0}"",""loc"":""{1}"",""weight"":{2},""popuptitle"":""{3}"",""popuphead"":""{4}"",""popupbody"":""{5}"""
            line = String.Format(line, p.ID, p.Location, p.Weight, p.PopupTitle, p.PopupHead, p.PopupBody)
            line = "{" + line + "}"

            function_params += String.Format("{0}{1}", split, line)
            split = ","
        Next
        function_params = "[" + function_params + "]"

        With postparam
            .Add(New KeyValuePair(Of String, String)("request_params", request_params))
            .Add(New KeyValuePair(Of String, String)("function_params", function_params))
            .Add(New KeyValuePair(Of String, String)("map_params", Map_P))
        End With

        Try
            trm = hc.PostAsync(New Uri(Routing_URL), New System.Net.Http.FormUrlEncodedContent(postparam))
            Dim rm As System.Net.Http.HttpResponseMessage
            rm = trm.Result
            If rm.IsSuccessStatusCode Then
                Dim ts As System.Threading.Tasks.Task(Of String)
                ts = rm.Content.ReadAsStringAsync
                Dim res As String = ts.Result
                If res <> "" Then
                    Routing_Result_JSON = res
                End If
            End If

        Catch ex As Exception
            ROUTING = False
            ResponseMessage = ex.Message
            Exit Function
        End Try

        '    RequestString = String.Format("{0}/{1}:{2}/xml?instructionsType=text&language={3}&key={4}", P.Routing_URL, P.Loc_FROM, P.Loc_TO, "hu-HU", API_key)
        '    Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(RequestString)
        '    Dim response As System.Net.HttpWebResponse
        '    Dim OUT As Boolean = False

        '    Try
        '        response = request.GetResponse()

        '    Catch ex As Exception
        '        ROUTING = False
        '        ResponseMessage = ex.Message
        '        Exit Function
        '    End Try

        '    If response.StatusCode = System.Net.HttpStatusCode.OK Then
        '        ResponseMessage = response.StatusCode

        '        Dim stream As System.IO.Stream = response.GetResponseStream()
        '        Dim reader As New System.IO.StreamReader(stream)
        '        Dim root As XElement = XDocument.Load(reader).Root
        '        Dim ns As XNamespace = "http://api.tomtom.com/routing"
        '        Dim items As IEnumerable(Of XElement) = root.Descendants(ns + "lengthInMeters")
        '        Dim leg As IEnumerable(Of XElement) = root.Descendants(ns + "leg")
        '        Dim point As IEnumerable(Of XElement) = leg.Descendants(ns + "point")
        '        Dim list As New List(Of String)

        '        For Each res As XElement In point
        '            list.Add(String.Format("{0},{1}", res.FirstAttribute.Value, res.LastAttribute.Value))
        '        Next
        '        ResponseValue = root.ToString

        '        Res_Distance_Orig = Val(items.First.FirstNode.ToString)
        '        Res_Distance_KM = Res_Distance_Orig / 1000
        '        Res_LngLat = list
        '    End If

        ROUTING = True
    End Function
End Class
