public interface IBlock
{
    bool IsSnow(); //return if check value false
    void Init(int block_num, bool snow);
    int Data { get; set; }
    bool Snow { get; set; }
    
}
    
