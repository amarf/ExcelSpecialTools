Attribute VB_Name = "Module1"
Public Delta As Double

Public Район As Double
Public Адрес As Double
Public ИндексПоРп As Double
Public ДопДанные As Double

Public ЭС As Double
Public ТС As Double
Public ГС As Double
Public ХВС As Double
Public ГВС As Double
Public ВО As Double
Public Фунд As Double
Public АППЗ As Double
Public Подвал As Double
Public Лифты As Double
Public Крыша As Double
Public Фасад As Double
Public Аварийка As Double
Public ПД As Double

Public Счетчик1 As Double
Public Счетчик2 As Double


Public Sub ВыполнитьПереносДанных()


    StopCalculation
    
    
    'Счетчик2 = -1
    
    'Счетчик1 = 1 'учитываем строку заголовков
    СтолбцыНовогоПлана
    trans 14, 2369, "Новый", "ррНовый"
    
    'Счетчик2 = 1 'учитываем строку заголовков
    СтолбцыСтарогоПлана
    trans 14, 1908, "238", "рр238"
    
    Module3.worked "ррНовый", "рр238"
    
    'Module2.Fined "ррНовый", "рр238"
    
    RunCalculation
End Sub

Public Function StopCalculation()
    Application.ScreenUpdating = False
    Application.Calculation = xlCalculationManual
End Function
Public Function RunCalculation()
    Application.ScreenUpdating = True
    Application.Calculation = xlCalculationAutomatic
End Function


Public Function СтолбцыНовогоПлана() '2015
    
    Район = 5
    Адрес = 7
    ИндексПоРп = 2
    ДопДанные = 3 'данные которые проще достать в пред обработке
    
    ЭС = 28
    ТС = 37
    ГС = 47
    ХВС = 58
    ГВС = 69
    ВО = 80
    Фунд = 90
    АППЗ = 97
    Подвал = 102
    Лифты = 110
    Крыша = 122
    Фасад = 133
    Аварийка = 144
    ПД = 148
End Function
Public Function СтолбцыСтарогоПлана()
    
    Район = 5
    Адрес = 7
    ИндексПоРп = 2
    ДопДанные = 3 'данные которые проще достать в пред обработке
    
    ЭС = 23
    ТС = 26
    ГС = 30
    ХВС = 35
    ГВС = 40
    ВО = 45
    Фунд = 50
    АППЗ = 54
    Подвал = 56
    Лифты = 60
    Крыша = 65
    Фасад = 70
    Аварийка = 75
    ПД = 77
End Function


Function trans(ByVal StartRow As Double, ByVal endRow As Double, ByVal DataListName As String, ByVal ResultListName As String)
    Dim ResultRow As Double
    ResultRow = 2

SetColumnHeaders ResultListName 'Заголовки


    Dim i As Double

    For i = StartRow To endRow
        If Worksheets(DataListName).Cells(i, Адрес) <> "" And Worksheets(DataListName).Cells(i, Район) <> "" Then
        
            SetData ResultListName, ResultRow + 0, "ЭС", DataListName, i, ЭС
            SetData ResultListName, ResultRow + 1, "ТС", DataListName, i, ТС
            SetData ResultListName, ResultRow + 2, "ГС", DataListName, i, ГС
            SetData ResultListName, ResultRow + 3, "ХВС", DataListName, i, ХВС
            SetData ResultListName, ResultRow + 4, "ГВС", DataListName, i, ГВС
            SetData ResultListName, ResultRow + 5, "ВО", DataListName, i, ВО
            SetData ResultListName, ResultRow + 6, "Фунд", DataListName, i, Фунд
            SetData ResultListName, ResultRow + 7, "АППЗ", DataListName, i, АППЗ
            SetData ResultListName, ResultRow + 8, "Подвал", DataListName, i, Подвал
            SetData ResultListName, ResultRow + 9, "Лифты", DataListName, i, Лифты
            SetData ResultListName, ResultRow + 10, "Крыша", DataListName, i, Крыша
            SetData ResultListName, ResultRow + 11, "Фасад", DataListName, i, Фасад
            SetData ResultListName, ResultRow + 12, "Аварийка", DataListName, i, Аварийка
            SetData ResultListName, ResultRow + 13, "ПД", DataListName, i, ПД
            
            ResultRow = ResultRow + 14
        Else: End If
    Next i
    


End Function



Function SetData( _
                        Имя_листа_с_отчетом As String, _
                        Номер_строки_отчета As Double, _
                        Наименование_работ As String, _
                        Имя_листа_с_данными As String, _
                        Номер_строки_с_данными As Double, _
                        Номер_столбца_с_данными As Double)

        Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 1) = Worksheets(Имя_листа_с_данными).Cells(Номер_строки_с_данными, Район)  'район
        Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 2) = Worksheets(Имя_листа_с_данными).Cells(Номер_строки_с_данными, Адрес)  'адрес
        Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 3) = Worksheets(Имя_листа_с_данными).Cells(Номер_строки_с_данными, ИндексПоРп)  'ИндексПоРп
        Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 4) = Worksheets(Имя_листа_с_данными).Cells(Номер_строки_с_данными, ДопДанные)  'дополнительные данные
        
        Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 5) = Наименование_работ  'Наименование работ
        Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 6) = Worksheets(Имя_листа_с_данными).Cells(Номер_строки_с_данными, Номер_столбца_с_данными) 'данные по стоимости
        Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 7) = Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 3) _
                                                                        & Worksheets(Имя_листа_с_отчетом).Cells(Номер_строки_отчета, 5) 'key

        'If Счетчик2 = -1 Then
        '    Счетчик1 = Счетчик1 + 1
        'Else
        '    Счетчик2 = Счетчик2 + 1
        'End If

End Function

Function SetColumnHeaders(Имя_листа_с_отчетом As String)
        Worksheets(Имя_листа_с_отчетом).Cells(1, 1) = "Район"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 2) = "Адрес"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 3) = "Позиция по РП"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 4) = "Дополнительные данные"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 5) = "Вид работ"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 6) = "Стоимость"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 7) = "Key"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 8) = "-"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 9) = "Старая стоимость"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 10) = "Примечание"
        Worksheets(Имя_листа_с_отчетом).Cells(1, 11) = "(Стоимость-Старая стоимость)"
        'Worksheets(Имя_листа_с_отчетом).Cells(1, 12) = "need delete"
End Function
