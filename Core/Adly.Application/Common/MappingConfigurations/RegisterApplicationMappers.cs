using AutoMapper;
using Microsoft.Win32;
using System.Reflection;

namespace Adly.Application.Common.MappingConfigurations;

public class RegisterApplicationMappers:Profile
{
    public RegisterApplicationMappers()
    {
        RegisterMappingProfiles(typeof(RegisterApplicationMappers).Assembly);
    }

    private void RegisterMappingProfiles(Assembly assembly)
    {
        var mappingTypes = assembly.GetTypes().Where(x => x.GetInterfaces().Any(m =>
            m.IsGenericType && m.GetGenericTypeDefinition() == typeof(ICreateApplicationMapper<>)));
        foreach (var type in mappingTypes)
        {
            var defaultConstructorLength = type.GetConstructors()
                .OrderByDescending(x=>x.GetParameters().Length)
                .First().GetParameters().Length;

            var model = Activator.CreateInstance(type, new object[defaultConstructorLength]);

            /*
             "ICreateApplicationMapper`1"
            
            `1 ==> یعنی این متود فقط یک پارامتر جنریک دارد مثلا اگر دوتا بود عدد یک می شد دو 

             */

            var methodInfo = type.GetMethod("Map") ?? type.GetInterface("ICreateApplicationMapper`1")!.GetMethod("Map");

            if (model is not null)
                methodInfo?.Invoke(model, [this]);

        }


    }
}