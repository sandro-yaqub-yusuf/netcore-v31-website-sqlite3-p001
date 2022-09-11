using System;
using System.Collections.Generic;
using System.Linq;
using KITAB.Products.CrossCutting.Utils;

namespace KITAB.Products.CrossCutting.Validations
{
    public class CPFValidacao
    {
        public const int TamanhoCPF = 11;

        public static bool Validar(string cpf)
        {
            string _cpfNumeros = Numeric.OnlyNumbers(cpf);

            if (!TamanhoValido(_cpfNumeros)) return false;

            return (!TemDigitosRepetidos(_cpfNumeros) && TemDigitosValidos(_cpfNumeros));
        }

        private static bool TamanhoValido(string valor)
        {
            return (valor.Length == TamanhoCPF);
        }

        private static bool TemDigitosRepetidos(string valor)
        {
            string[] _invalidNumbers =
            {
                "00000000000",
                "11111111111",
                "22222222222",
                "33333333333",
                "44444444444",
                "55555555555",
                "66666666666",
                "77777777777",
                "88888888888",
                "99999999999"
            };

            return _invalidNumbers.Contains(valor);
        }

        private static bool TemDigitosValidos(string valor)
        {
            string _number = valor.Substring(0, (TamanhoCPF - 2));

            var _digitoVerificador = new DigitoVerificador(_number).ComMultiplicadoresDeAte(2, 11).Substituindo("0", 10, 11);

            string _firstDigit = _digitoVerificador.CalculaDigito();

            _digitoVerificador.AddDigito(_firstDigit);

            string _secondDigit = _digitoVerificador.CalculaDigito();

            return string.Concat(_firstDigit, _secondDigit) == valor.Substring((TamanhoCPF - 2), 2);
        }
    }

    public class CNPJValidacao
    {
        public const int TamanhoCNPJ = 14;

        public static bool Validar(string cpnj)
        {
            string _cnpjNumeros = Numeric.OnlyNumbers(cpnj);

            if (!TemTamanhoValido(_cnpjNumeros)) return false;

            return (!TemDigitosRepetidos(_cnpjNumeros) && TemDigitosValidos(_cnpjNumeros));
        }

        private static bool TemTamanhoValido(string valor)
        {
            return (valor.Length == TamanhoCNPJ);
        }

        private static bool TemDigitosRepetidos(string valor)
        {
            string[] _invalidNumbers =
            {
                "00000000000000",
                "11111111111111",
                "22222222222222",
                "33333333333333",
                "44444444444444",
                "55555555555555",
                "66666666666666",
                "77777777777777",
                "88888888888888",
                "99999999999999"
            };

            return _invalidNumbers.Contains(valor);
        }

        private static bool TemDigitosValidos(string valor)
        {
            string _number = valor.Substring(0, (TamanhoCNPJ - 2));

            DigitoVerificador _digitoVerificador = new DigitoVerificador(_number).ComMultiplicadoresDeAte(2, 9).Substituindo("0", 10, 11);

            string _firstDigit = _digitoVerificador.CalculaDigito();

            _digitoVerificador.AddDigito(_firstDigit);

            string _secondDigit = _digitoVerificador.CalculaDigito();

            return string.Concat(_firstDigit, _secondDigit) == valor.Substring((TamanhoCNPJ - 2), 2);
        }
    }

    public class DigitoVerificador
    {
        private string _numero;
        private const int _modulo = 11;
        private readonly List<int> _multiplicadores = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };
        private readonly IDictionary<int, string> _substituicoes = new Dictionary<int, string>();
        private readonly bool _complementarDoModulo = true;

        public DigitoVerificador(string numero)
        {
            _numero = numero;
        }

        public DigitoVerificador ComMultiplicadoresDeAte(int primeiroMultiplicador, int ultimoMultiplicador)
        {
            _multiplicadores.Clear();

            for (int i = primeiroMultiplicador; i <= ultimoMultiplicador; i++) 
            {
                _multiplicadores.Add(i);
            }

            return this;
        }

        public DigitoVerificador Substituindo(string substituto, params int[] digitos)
        {
            foreach (int i in digitos)
            {
                _substituicoes[i] = substituto;
            }

            return this;
        }

        public void AddDigito(string digito)
        {
            _numero = string.Concat(_numero, digito);
        }

        public string CalculaDigito()
        {
            return (!(_numero.Length > 0) ? "" : GetDigitSum());
        }

        private string GetDigitSum()
        {
            int _soma = 0;

            for (int i = _numero.Length - 1, m = 0; i >= 0; i--)
            {
                int _produto = ((int)char.GetNumericValue(_numero[i]) * _multiplicadores[m]);

                _soma += _produto;

                if (++m >= _multiplicadores.Count) m = 0;
            }

            int _mod = (_soma % _modulo);
            int _resultado = (_complementarDoModulo ? (_modulo - _mod) : _mod);

            return (_substituicoes.ContainsKey(_resultado) ? _substituicoes[_resultado] : _resultado.ToString());
        }
    }
}
