import "../styles/Bucket.css";

interface BucketProps {
  name: string;
  icon: string;
  amount?: number;
  id?: number;
}

const BucketCard = ({ id,name, icon, amount }: BucketProps) => {
  return (
    <div className="bucket-card" key={id}>
      <p id="icon">{icon}</p>
      <p id="name">{name}</p>
      <p id="amount">Total: €{amount}</p>
    </div>
  );
};
export default BucketCard;
// React.FC<BucketProps> = ({props}) :ReactElement is niet nodig.
// JSX.Element is ook niet nodig, TS inferred the type van de component return.
// :BucketProps geeft types aan van de props.
// React.ChangeEvent<HTMLInputElement>
// Number() is nog steeds nodig als je met numeric values operations doet.
