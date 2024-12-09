namespace library_management.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<BorrowingDto> Borrowings { get; set; }
    }

}
