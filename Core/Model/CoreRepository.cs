using Core.Interfaces;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    [ImplementPropertyChanged]
    public class CoreRepository : IRepository
    {
        private CoreOption _coreOptions;
        private readonly IDataService _dataService;
        private readonly IErrorTraceService _errorTraceService;
        private readonly IErrorMessageService _errorMessageService;

        public CoreRepository(IDataService dataService, IErrorTraceService errorTraceService, IErrorMessageService errorMessageService)
        {
            _dataService = dataService;
            _errorTraceService = errorTraceService;
            _errorMessageService = errorMessageService;

            IsLoadOption = OptionLoad();
            DataList = new ObservableCollection<object>();
        }


        #region Property

        public bool IsLoadOption { get; private set; } 
        ObservableCollection<object> DataList { get;  set; }

        #endregion

        private bool OptionLoad()
        {
            if (File.Exists(CoreOption.FileFullName))
            {
                try
                {
                    _coreOptions = _dataService.DeserializeObject<CoreOption>(CoreOption.FileFullName);
                    return true;
                }
                catch (Exception e)
                {
                    _errorTraceService.Trace(e, "В файле настроек содержатся ошибки. Файл не был загружен.");
                }
            }
            else
            {
                try
                {
                    _coreOptions = new CoreOption();
                    _dataService.SerializeObject<CoreOption>(_coreOptions, CoreOption.FileFullName);
                    return true;
                }
                catch (Exception e)
                {
                    _coreOptions = null;
                    _errorTraceService.Trace(e, "Не удалось создать файл настроек. Проверти доступна ли директория \"Документы\".");
                }
            }
            return false;
        }

        public DataType GetData<DataType>(string dataPath = null) where DataType : class
        {
            DataType data = null;
            if (!string.IsNullOrWhiteSpace(dataPath)) //указываем явно что хоти получить данных из источника, а не из памяти
            {
                try
                {
                    data = _dataService.DeserializeObject<DataType>(dataPath);
                }
                catch (Exception e)
                {
                    _errorTraceService.Trace(e, $"Не удалось загрузить данные из {dataPath}");
                }
            }
            else //если нет пути то пробуем его получить объект из памяти, и если его там нет то пробуем загрузить по пути из настроек
            {
                data = DataList.LastOrDefault(i => i.GetType() == typeof(DataType)) as DataType;
                if (data == null && !string.IsNullOrWhiteSpace(_coreOptions[typeof(DataType)]))
                {
                    //пытаемся загрузить по пути
                    try
                    {
                        data = _dataService.DeserializeObject<DataType>(dataPath);
                    }
                    catch (Exception e)
                    {
                        _errorTraceService.Trace(e, $"Не удалось загрузить данные из {dataPath}");
                    }
                }
            }
           
            return data;
        }
        public void SetData<DataType>(DataType data, string dataPath = null) where DataType : class
        {
            string savepath = dataPath ?? _coreOptions[typeof(DataType)];
            if (!string.IsNullOrWhiteSpace(savepath))
            {
                try
                {
                    _dataService.SerializeObject(data, savepath);
                }
                catch (Exception e)
                {
                    _errorTraceService.Trace(e,  $"Не удалось сохранить данные [{typeof(DataType).Name}]");
                }

                var oldData = DataList.FirstOrDefault(i => i.GetType() == typeof(DataType)) as DataType;
                if (oldData != null)
                    DataList.Remove(oldData);
                DataList.Add(data);
                return;
            }

            _errorMessageService.Send($"Не удалось сохранить данные [{typeof(DataType).Name}] т.к. приложению не удалось определить путь");
        }
        public DataType ReloadData<DataType>(string dataPath = null)
        {
            throw new NotImplementedException();
        }

        public string GetOption(string optionName)
        {
            if (IsLoadOption)
                return _coreOptions[optionName];
            else
                return null;
        }
        public bool SetOption(string optionName, string optionValue)
        {
            if (IsLoadOption)
            {
                _coreOptions[optionName] = optionValue;
                return true;
            }
            else
                return false;
        }

        public DataType GetData<DataType>()
        {
            throw new NotImplementedException();
        }
    }
}
