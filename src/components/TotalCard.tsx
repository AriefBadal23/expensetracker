import "../styles/Bucket.css";
import "../styles/BucketList.css";

interface ITotalCard{
    name:string,
    amount: number,
    icon: string
}
const TotalCard = ({name, amount, icon}):ITotalCard => {
    return (
        <div className="bucket-card">
            <p id="icon">{icon}</p>
            <p id="name">{name}</p>
            <p id="amount">Total: €{amount}</p>
        </div>
        
    )
}
export default TotalCard