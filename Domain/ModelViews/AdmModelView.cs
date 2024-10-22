

namespace minimal_api.Domain.ModelViews
{
    public record AdmModelView
    {
        public int Id { get; set;}
        public string Email { get; set; } = default!;
        public string Profile { get; set; } = default!;
    }
}