Attribute VB_Name = "Module3"




Public Function worked(newDataSheetName As String, oldDataSheetName As String)
    
    Dim rowCountNew As Double, rowCountOld As Double
    
    Module1.RunCalculation
    
    rowCountNew = GetRowCount(newDataSheetName)
    rowCountOld = GetRowCount(oldDataSheetName)
    SortDataTable newDataSheetName, rowCountNew
    SortDataTable oldDataSheetName, rowCountOld
    
    Module1.StopCalculation
    
    whileWork rowCountNew, rowCountOld, newDataSheetName, oldDataSheetName
    moveData newDataSheetName, rowCountNew, oldDataSheetName, rowCountOld
    

End Function





Public Function whileWork(rowCountNew As Double, rowCountOld As Double, newDataSheetName As String, oldDataSheetName As String)
    Dim newRowNumber As Double, oldRowNumber As Double
    Dim newIndexRp As Double, oldIndexRp As Double
    Dim newValue As String, oldValue As String
    newRowNumber = 2
    oldRowNumber = 2
    
    Do While newRowNumber <= rowCountNew
    
        newIndexRp = Worksheets(newDataSheetName).Cells(newRowNumber, 3)
        oldIndexRp = Worksheets(oldDataSheetName).Cells(oldRowNumber, 3)
        newWork = Worksheets(newDataSheetName).Cells(newRowNumber, 5)
        oldWork = Worksheets(oldDataSheetName).Cells(oldRowNumber, 5)
        newKey = newIndexRp & newWork
        oldKey = oldIndexRp & oldWork
        
        If newKey = oldKey Then
            newValue = Worksheets(newDataSheetName).Cells(newRowNumber, 6)
            oldValue = Worksheets(oldDataSheetName).Cells(oldRowNumber, 6)
            
            SetResultData newDataSheetName, newRowNumber, oldValue, "find", ""
            SetResultData oldDataSheetName, oldRowNumber, newValue, "find", ""
            
            If (oldRowNumber = rowCountOld) Or (newRowNumber = rowCountNew) Then
                GoTo finalize 'когда задали последнее значени уходим из цикла
            End If
            
            oldRowNumber = oldRowNumber + 1
            newRowNumber = newRowNumber + 1
        
        ElseIf (newKey <> oldKey And oldRowNumber = rowCountNew) Or (newKey <> oldKey And newRowNumber = rowCountOld) Then
            GoTo finalize 'выход когда достигли споследнего значения и оно notfound
        Else
        
            If newIndexRp > oldIndexRp Then
                oldRowNumber = IncrementIndex(newIndexRp, oldRowNumber, rowCountOld, oldDataSheetName)
            Else
                newRowNumber = IncrementIndex(oldIndexRp, newRowNumber, rowCountNew, newDataSheetName)
            End If
            
        End If
    
    Loop
    
finalize: 'кто то один всегда дойдет до конца
    If (oldRowNumber = rowCountOld) Then
        Do
            newRowNumber = newRowNumber + 1
            SetResultData newDataSheetName, newRowNumber, "", "not found finalize", ""
        Loop Until newRowNumber = rowCountNew
    Else
        Do
            oldRowNumber = oldRowNumber + 1
            SetResultData oldDataSheetName, oldRowNumber, "", "not found finalize", ""
        Loop Until oldRowNumber = rowCountOld
    End If
    
    
    

End Function


'!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
Public Function IncrementIndex(IndexRp As Double, outRowNumber As Double, outRowCount As Double, outSheetName As String) As Double
    Dim returRowNumber As Double, outIndexRp As Double
    'инкрементируем внешний лист пока не выровним индексы
    'возращаем номер строки которая соответствует индексу
    returRowNumber = outRowNumber
    outIndexRp = Worksheets(outSheetName).Cells(returRowNumber, 3) 'получаем первый раз индекс
    
    Do While IndexRp > outIndexRp And outRowNumber < outRowCount
        SetResultData outSheetName, returRowNumber, "0", "not found in out source", ""
        returRowNumber = returRowNumber + 1
        outIndexRp = Worksheets(outSheetName).Cells(returRowNumber, 3)
    Loop
    
    IncrementIndex = returRowNumber
    
End Function

Public Function Test(val As String) As Variant

  Test = WorksheetFunction.Text(val, "General")

End Function



Public Function worked2222()

worked "рр238", "ррНовый"

End Function

Public Function moveData(dataSheetName As String, dataSheetRowsCount As Double, sourceDataSheetName As String, sourceSheetRowsCount As Double)

    Dim lastRowInDataSheet As Double
    lastRowInDataSheet = dataSheetRowsCount
    
    For i = 2 To sourceSheetRowsCount
    
        flag = Worksheets(sourceDataSheetName).Cells(i, 10)
        If flag <> "find" Then
            
            Worksheets(dataSheetName).Cells(lastRowInDataSheet, 1) = Worksheets(sourceDataSheetName).Cells(i, 1)
            Worksheets(dataSheetName).Cells(lastRowInDataSheet, 2) = Worksheets(sourceDataSheetName).Cells(i, 2)
            Worksheets(dataSheetName).Cells(lastRowInDataSheet, 3) = Worksheets(sourceDataSheetName).Cells(i, 3)
            Worksheets(dataSheetName).Cells(lastRowInDataSheet, 4) = Worksheets(sourceDataSheetName).Cells(i, 4)
            Worksheets(dataSheetName).Cells(lastRowInDataSheet, 5) = Worksheets(sourceDataSheetName).Cells(i, 5)
            Worksheets(dataSheetName).Cells(lastRowInDataSheet, 6) = Worksheets(sourceDataSheetName).Cells(i, 8)  'новая стоимость уже 0
            Worksheets(dataSheetName).Cells(lastRowInDataSheet, 7) = Worksheets(sourceDataSheetName).Cells(i, 7)
            Worksheets(dataSheetName).Cells(lastRowInDataSheet, 8) = Worksheets(sourceDataSheetName).Cells(i, 8)
            Worksheets(dataSheetName).Cells(lastRowInDataSheet, 9) = Worksheets(sourceDataSheetName).Cells(i, 6) 'стоимость которая была
            Worksheets(dataSheetName).Cells(lastRowInDataSheet, 10) = "Remove Object"
            
            cost = Worksheets(dataSheetName).Cells(lastRowInDataSheet, 6)
            oldcost = Worksheets(dataSheetName).Cells(lastRowInDataSheet, 9)
            If cost = 0 And oldcost = 0 Then
                Worksheets(dataSheetName).Cells(lastRowInDataSheet, 10) = "need delete"
            Else
                Worksheets(dataSheetName).Cells(lastRowInDataSheet, 11) = cost - oldcost
            End If
            
            lastRowInDataSheet = lastRowInDataSheet + 1
            
        End If
    
    Next i
    
    
    For i = 2 To dataSheetRowsCount
        
        
        flag = Worksheets(dataSheetName).Cells(i, 10)
        cost = Worksheets(dataSheetName).Cells(i, 6)
        oldcost = Worksheets(dataSheetName).Cells(i, 9)
        
        If Len(cost) = 0 Then Worksheets(dataSheetName).Cells(i, 6) = 0 'удибраем пустые
       
        If cost = 0 And oldcost = 0 Then
            Worksheets(dataSheetName).Cells(i, 10) = "need delete"
         Else
            Worksheets(dataSheetName).Cells(i, 11) = cost - oldcost
            If cost = 0 And oldcost <> 0 And flag = "find" Then Worksheets(dataSheetName).Cells(i, 10) = "remove work"
            If cost <> 0 And oldcost = 0 And flag = "find" Then Worksheets(dataSheetName).Cells(i, 10) = "add work"
        End If
            
        If flag = "not found in out source" Or flag = "not found finalize" Then
            If cost <> 0 And oldcost <> 0 Then Worksheets(dataSheetName).Cells(i, 10) = "Add Object" 'TODO отладка
        End If
    Next i
End Function

Public Function SetResultData(refSheetName As String, rowNumber As Double, value As String, flag As String, comment As String)

        Dim newValue As Double

        If Len(value) Then
            newValue = CDbl(value + 0)
        Else
            newValue = 0
        End If

        'Worksheets(refSheetName).Cells(rowNumber, 8) = rowNumber
        Worksheets(refSheetName).Cells(rowNumber, 9) = newValue  'WorksheetFunction.Sum(0, value)
        Worksheets(refSheetName).Cells(rowNumber, 10) = flag
        Worksheets(refSheetName).Cells(rowNumber, 13) = comment

End Function


Public Function GetRowCount(sheetName As String) As Double
    Worksheets(sheetName).Select
    Worksheets(sheetName).Cells(1, 1).Select
    Selection.End(xlDown).Select
    GetRowCount = ActiveCell.Row
End Function

Public Function SortDataTable(sheetName As String, endRow As Double)
    ActiveWorkbook.Worksheets(sheetName).sort.SortFields.Clear
    ActiveWorkbook.Worksheets(sheetName).sort.SortFields.Add Key:=Range("C2:C" & endRow) _
        , SortOn:=xlSortOnValues, Order:=xlAscending, DataOption:=xlSortNormal
    ActiveWorkbook.Worksheets(sheetName).sort.SortFields.Add Key:=Range("E2:E" & endRow) _
        , SortOn:=xlSortOnValues, Order:=xlAscending, DataOption:=xlSortNormal
    With ActiveWorkbook.Worksheets(sheetName).sort
        .SetRange Range("A1:M" & endRow)
        .Header = xlYes
        .MatchCase = False
        .Orientation = xlTopToBottom
        .SortMethod = xlPinYin
        .Apply
    End With
End Function

