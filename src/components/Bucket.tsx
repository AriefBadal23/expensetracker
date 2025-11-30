import "../styles/Bucket.css";

interface BucketProps {
  name: string;
  icon: string;
  amount: number;
}

const Bucket = ({ name, icon, amount }: BucketProps) => {
  return (
    <div className="bucket-card">
      <p id="icon">{icon}</p>
      <p id="name">{name}</p>
      <p id="amount">€{amount}</p>
    </div>
  );
};
export default Bucket;
// React.FC<BucketProps> = ({props})
// :ReactElement is niet nodig.
// JSX.Element is ook niet nodig, TS inferred the type van de component return.
// :BucketProps geeft types aan van de props.
// React.ChangeEvent<HTMLInputElement>