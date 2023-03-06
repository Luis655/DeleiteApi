using Deleite.Dal.Interfaces;
using Deleite.Entity.DtoModels;
using FluentValidation;

namespace Deleite.Bll.Validators
{

    public class CreateLoginRequestValidators : AbstractValidator<DtoUserLogin>
    {
        private readonly IGenericRepository<DtoUserLogin> _repository;

        public CreateLoginRequestValidators(IGenericRepository<DtoUserLogin> repository)
        {
            this._repository = repository;


            RuleFor(x => x.Correo).NotEmpty().WithMessage("El correo es requerid.")
            .NotNull().WithMessage("El correo no puede ser nula.")
            .Length(5, 30).WithMessage("El correo debe tener entre 5 y 30 caracteres.")
            .Must(NoContieneSqlInjection)
            .WithMessage("El correo no puede contener palabras reservadas de SQL")
            //.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{5,15}$")
            //.WithMessage("El correo debe contener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")
            .Matches("^[a-zA-Z0-9@.]+$")
            .WithMessage("El correo solo puede contener letras y números."); 



            RuleFor(x => x.Contraseña).NotEmpty().WithMessage("La contraseña es requerida.")
            .NotNull().WithMessage("La contraseña no puede ser nula.")
            .Length(5, 30).WithMessage("La contraseña debe tener entre 5 y 30 caracteres.")
            .Must(NoContieneSqlInjection)
            .WithMessage("La contraseña no puede contener palabras reservadas de SQL")
            //.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{5,15}$")
            //.WithMessage("La contraseña debe contener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")
            .Matches("^[a-zA-Z0-9]+$")
            .WithMessage("La contraseña solo puede contener letras y números."); 
        }
        private static bool NoContieneSqlInjection(string contraseña)
        {
            // Lista de palabras reservadas de SQL
            var palabrasReservadas = new[] { "SELECT", "INSERT", "UPDATE", "DELETE", "DROP", "TRUNCATE", "ALTER", "CREATE", "EXEC", "EXECUTE", "SP_", "XP_", "--" };
            // Comprobar si la contraseña contiene alguna palabra reservada de SQL
            return !palabrasReservadas.Any(palabra => contraseña.ToUpper().Contains(palabra));
        }
    }
}