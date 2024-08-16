
using FluentValidation;
using LibraryInventoryAPI.Models.DTOs;

public class BookCreateValidator : AbstractValidator<BookCreateDto>
{
    public BookCreateValidator()
    {
        RuleFor(book => book.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(book => book.Author)
            .NotEmpty().WithMessage("Author is required.")
            .MaximumLength(100).WithMessage("Author name cannot exceed 100 characters.");

        RuleFor(book => book.PublicationYear)
            .InclusiveBetween(1500, DateTime.Now.Year)
            .WithMessage("Publication year must be between 1500 and the current year.");
    }
}
