using Core.Interfaces;
using ExcelAnalysisTools.Model;
using Microsoft.Practices.ServiceLocation;
using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAnalysisTools.Services
{
    [ImplementPropertyChanged]
    public class Repository
    {
        private readonly IDataService _dataService;
        private readonly IUserMsgService _userMsgService;
        private readonly IServiceLocator _serviceLocator;

        public Repository(IDataService dataService, IUserMsgService userMsgService, IServiceLocator serviceLocator)
        {
            _dataService = dataService;
            _userMsgService = userMsgService;
            _serviceLocator = serviceLocator;
        }

        AddressList _addressList;
        RegexExpressionList _regexList;
        Options _options;
        ProfileList _profileList;

        public AddressList AddressList { get { return GetIns(ref _addressList); } private set { _addressList = value; } }
        public RegexExpressionList RegexList { get { return GetIns(ref _regexList); } private set { _regexList = value; } }
        public Options Options { get { return GetIns(ref _options);} private set { _options = value; } }
        public ProfileList ProfileList { get { return GetIns(ref _profileList); } private set { _profileList = value; } }

        public void Save<T>() where T : class => TrySaveData<T>(Options.GetDataPath<T>());
        public T Create<T>(string path) where T : class => TryCreateData<T>(path);
        public T Load<T>(string path) where T : class => TryLoadData<T>(path);

        private T GetIns<T>(ref T obj) where T : class
        {
            if (obj == null)
            {
                if (typeof(T) == typeof(Options) && !string.IsNullOrWhiteSpace(Options.OptionsFileFullPath))
                {
                    try
                    {
                        obj = _dataService.DeserializeObject<T>(Options.OptionsFileFullPath);
                    }
                    catch (Exception)
                    {
                        obj = TryCreateData<T>(Options.OptionsFileFullPath);
                    }
                    finally
                    {
                        (obj as INotifyPropertyChanged).PropertyChanged += (sender, args) =>
                        {
                            if(args.PropertyName == "AddressListPath")
                                AddressList = TryLoadData<AddressList>(Options.GetDataPath<AddressList>());
                            else if (args.PropertyName == "RegexListPath")
                                RegexList = TryLoadData<RegexExpressionList>(Options.GetDataPath<RegexExpressionList>());
                            else if (args.PropertyName == "ProfileListPath")
                                ProfileList = TryLoadData<ProfileList>(Options.GetDataPath<ProfileList>());
                        };
                    }
                    return obj;
                }
                else
                    return TryLoadData<T>(Options.GetDataPath<T>());
            }
            else
                return obj;
        }
        private T TryLoadData<T>(string path) where T : class
        {
            try
            {
                var data = _dataService.DeserializeObject<T>(path);
                Options.SetDataPath<T>(path);
                TrySaveData<Options>(Options.OptionsFileFullPath);

                SetData<T>(data);
                return data;
            }
            catch (Exception e)
            {
                _userMsgService.MsgShow("Не удалось загрузить данные: " + typeof(T).Name);
                return null;
            }
        }
        private void TrySaveData<T>(string path) where T : class
        {
            var obj = GetData<T>();
            if (obj == null) return;

            try
            {
                _dataService.SerializeObject<T>(obj, path);
            }
            catch (Exception e)
            {
                _userMsgService.MsgShow("Не удалось сохранить данные: " + typeof(T).Name);
            }
        }
        private T TryCreateData<T>(string path) where T : class
        {
            try
            {
                //var m = typeof(T).GetMethod("Create", BindingFlags.Static);

                var data = _serviceLocator.GetInstance<T>();
                SetData<T>(data);
                TrySaveData<T>(path);
                Options.SetDataPath<T>(path);
                TrySaveData<Options>(Options.OptionsFileFullPath);
                return data;
            }
            catch (Exception e)
            {
                _userMsgService.MsgShow("Не удалось создать данные: " + typeof(T).Name);
                return null;
            }
        }


        public T GetData<T>() where T: class
        {
            var t = this.GetType().GetProperties().FirstOrDefault(property => property.PropertyType == typeof(T));
            return t != null ? (T)t.GetValue(this) : null;
        }
        public void SetData<T>(T data) where T : class
        {
            var t = this.GetType().GetProperties().FirstOrDefault(property => property.PropertyType == typeof(T));
            if (t != null) t.SetValue(this, data);
        }


    }


}
