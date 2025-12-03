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
    <tr className={`${bucket === Buckets.Salary} ? "table-success": ""`}>
      <td className={`${bucket === Buckets.Salary} ? "table-light": ""`}>
        {description}
      </td>
      <td className={`${bucket === Buckets.Salary} ? "table-light": ""`}>
        €{amount}
      </td>
      <td className={`${bucket === Buckets.Salary} ? "table-light": ""`}>
        {Buckets[bucket as keyof typeof Buckets]}
      </td>
    </tr>
  );
};

export default TransactionRow;
