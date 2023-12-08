public class EngineSchematicEntry
{
    public int X {get;set;}
    public int Y {get;set;}
    public string Characters {get;set;} = string.Empty;
    
    public int Length { get { return Characters.Length;}}
    public bool IsSymbol
    {
        get
        {
            // return(Characters.Length == 1) && (Characters[0] != '.');
            return !IsNumber;
        }
    }

    public bool IsNumber
    {
        get
        {
            return Int32.TryParse(Characters, out _);
        }
    }
    public int Number
    {
        get
        {
            if(!IsNumber) throw new Exception("take off eh");
            return Int32.Parse(Characters);
        }
    }

    public bool IsGearSymbol
    {
        get
        {
            return !IsNumber && Characters == "*";
        }
    }

    public bool IsAdjacent(EngineSchematicEntry other)
    {
        // Console.WriteLine($"this.X:{this.X} this.Y:{this.Y} other.X:{other.X} other.Y:{other.Y}");
        //logic written assuming this is a number, and other is a symbol
        if(this.IsSymbol)
            return false;
        
        if(other.IsNumber)
            return false;

        //if the other is too many rows away, exit early
        if(other.Y < this.Y-1) return false;
        if(other.Y > this.Y+1) return false;

        //deal with same row case
        if(this.Y == other.Y)
        {
            if(this.X == other.X+1) return true;
            if(this.X + this.Length == other.X) return true;
        }
        
        //now we only have to check the +/- rows
        //if the other symbol overlaps this.X+lenght +/- 1, it's adjacent

        if( other.X < this.X-1) return false;
        if( other.X > (this.X+this.Length)) return false;

        // Console.WriteLine(" Returning true");
        return true;
    }
}