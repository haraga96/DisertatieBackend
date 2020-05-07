using System;
namespace Backend_Dis_App.Validators
{
    public interface IPasswordValidator
    {
        bool CheckRule(object parameter);
    }
}
