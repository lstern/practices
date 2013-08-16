﻿namespace MTO.Practices.Templating.Lexer.Interfaces
{
    using MTO.Practices.Templating.Lexer.Enumerators;

    /// <summary>
    /// Interface comum a elementos de template da engine
    /// </summary>
    public interface ITemplateElement
    {
        /// <summary>
        /// Nome da tag
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Estado atual da tag
        /// </summary>
        ElementStatusEnum ElementStatus { get; set; }

        /// <summary>
        /// Processa conteúdo recebido durante o contexto de processamento do elemento
        /// </summary>
        /// <param name="content">O conteúdo</param>
        /// <returns>O resultado</returns>
        string ProcessContent(string content);

        /// <summary>
        /// Adiciona um novo argumetno
        /// </summary>
        /// <param name="name">Nome do argumento</param>
        /// <param name="value">Valor do argumento</param>
        void AddArgument(string name, string value);

        /// <summary>
        /// Inicia novo argumento quando nao sabemos nome nem valor
        /// </summary>
        void StartArgument();
    }
}