import { Buckets } from "../types/Buckets";

interface TransactionRowProps {
  description: string;
  amount: number;
  bucket: string;
}

const TransactionRow = ({
  description,
  amount,
  bucket,
}: TransactionRowProps) => {
  return (
    <tr>
      <td>{description}</td>
      <td>€{amount}</td>
      <td>{Buckets[bucket as keyof typeof Buckets]}</td>
    </tr>
  );
};

export default TransactionRow;
