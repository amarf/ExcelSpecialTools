Attribute VB_Name = "Module1"
Public Delta As Double

Public ����� As Double
Public ����� As Double
Public ���������� As Double
Public ��������� As Double

Public �� As Double
Public �� As Double
Public �� As Double
Public ��� As Double
Public ��� As Double
Public �� As Double
Public ���� As Double
Public ���� As Double
Public ������ As Double
Public ����� As Double
Public ����� As Double
Public ����� As Double
Public �������� As Double
Public �� As Double

Public �������1 As Double
Public �������2 As Double


Public Sub ����������������������()


    StopCalculation
    
    
    '�������2 = -1
    
    '�������1 = 1 '��������� ������ ����������
    ������������������
    trans 14, 2369, "�����", "�������"
    
    '�������2 = 1 '��������� ������ ����������
    �������������������
    trans 14, 1908, "238", "��238"
    
    Module3.worked "�������", "��238"
    
    'Module2.Fined "�������", "��238"
    
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


Public Function ������������������() '2015
    
    ����� = 5
    ����� = 7
    ���������� = 2
    ��������� = 3 '������ ������� ����� ������� � ���� ���������
    
    �� = 28
    �� = 37
    �� = 47
    ��� = 58
    ��� = 69
    �� = 80
    ���� = 90
    ���� = 97
    ������ = 102
    ����� = 110
    ����� = 122
    ����� = 133
    �������� = 144
    �� = 148
End Function
Public Function �������������������()
    
    ����� = 5
    ����� = 7
    ���������� = 2
    ��������� = 3 '������ ������� ����� ������� � ���� ���������
    
    �� = 23
    �� = 26
    �� = 30
    ��� = 35
    ��� = 40
    �� = 45
    ���� = 50
    ���� = 54
    ������ = 56
    ����� = 60
    ����� = 65
    ����� = 70
    �������� = 75
    �� = 77
End Function


Function trans(ByVal StartRow As Double, ByVal endRow As Double, ByVal DataListName As String, ByVal ResultListName As String)
    Dim ResultRow As Double
    ResultRow = 2

SetColumnHeaders ResultListName '���������


    Dim i As Double

    For i = StartRow To endRow
        If Worksheets(DataListName).Cells(i, �����) <> "" And Worksheets(DataListName).Cells(i, �����) <> "" Then
        
            SetData ResultListName, ResultRow + 0, "��", DataListName, i, ��
            SetData ResultListName, ResultRow + 1, "��", DataListName, i, ��
            SetData ResultListName, ResultRow + 2, "��", DataListName, i, ��
            SetData ResultListName, ResultRow + 3, "���", DataListName, i, ���
            SetData ResultListName, ResultRow + 4, "���", DataListName, i, ���
            SetData ResultListName, ResultRow + 5, "��", DataListName, i, ��
            SetData ResultListName, ResultRow + 6, "����", DataListName, i, ����
            SetData ResultListName, ResultRow + 7, "����", DataListName, i, ����
            SetData ResultListName, ResultRow + 8, "������", DataListName, i, ������
            SetData ResultListName, ResultRow + 9, "�����", DataListName, i, �����
            SetData ResultListName, ResultRow + 10, "�����", DataListName, i, �����
            SetData ResultListName, ResultRow + 11, "�����", DataListName, i, �����
            SetData ResultListName, ResultRow + 12, "��������", DataListName, i, ��������
            SetData ResultListName, ResultRow + 13, "��", DataListName, i, ��
            
            ResultRow = ResultRow + 14
        Else: End If
    Next i
    


End Function



Function SetData( _
                        ���_�����_�_������� As String, _
                        �����_������_������ As Double, _
                        ������������_����� As String, _
                        ���_�����_�_������� As String, _
                        �����_������_�_������� As Double, _
                        �����_�������_�_������� As Double)

        Worksheets(���_�����_�_�������).Cells(�����_������_������, 1) = Worksheets(���_�����_�_�������).Cells(�����_������_�_�������, �����)  '�����
        Worksheets(���_�����_�_�������).Cells(�����_������_������, 2) = Worksheets(���_�����_�_�������).Cells(�����_������_�_�������, �����)  '�����
        Worksheets(���_�����_�_�������).Cells(�����_������_������, 3) = Worksheets(���_�����_�_�������).Cells(�����_������_�_�������, ����������)  '����������
        Worksheets(���_�����_�_�������).Cells(�����_������_������, 4) = Worksheets(���_�����_�_�������).Cells(�����_������_�_�������, ���������)  '�������������� ������
        
        Worksheets(���_�����_�_�������).Cells(�����_������_������, 5) = ������������_�����  '������������ �����
        Worksheets(���_�����_�_�������).Cells(�����_������_������, 6) = Worksheets(���_�����_�_�������).Cells(�����_������_�_�������, �����_�������_�_�������) '������ �� ���������
        Worksheets(���_�����_�_�������).Cells(�����_������_������, 7) = Worksheets(���_�����_�_�������).Cells(�����_������_������, 3) _
                                                                        & Worksheets(���_�����_�_�������).Cells(�����_������_������, 5) 'key

        'If �������2 = -1 Then
        '    �������1 = �������1 + 1
        'Else
        '    �������2 = �������2 + 1
        'End If

End Function

Function SetColumnHeaders(���_�����_�_������� As String)
        Worksheets(���_�����_�_�������).Cells(1, 1) = "�����"
        Worksheets(���_�����_�_�������).Cells(1, 2) = "�����"
        Worksheets(���_�����_�_�������).Cells(1, 3) = "������� �� ��"
        Worksheets(���_�����_�_�������).Cells(1, 4) = "�������������� ������"
        Worksheets(���_�����_�_�������).Cells(1, 5) = "��� �����"
        Worksheets(���_�����_�_�������).Cells(1, 6) = "���������"
        Worksheets(���_�����_�_�������).Cells(1, 7) = "Key"
        Worksheets(���_�����_�_�������).Cells(1, 8) = "-"
        Worksheets(���_�����_�_�������).Cells(1, 9) = "������ ���������"
        Worksheets(���_�����_�_�������).Cells(1, 10) = "����������"
        Worksheets(���_�����_�_�������).Cells(1, 11) = "(���������-������ ���������)"
        'Worksheets(���_�����_�_�������).Cells(1, 12) = "need delete"
End Function
