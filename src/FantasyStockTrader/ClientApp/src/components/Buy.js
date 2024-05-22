import axios from "axios";
import React, { useEffect, useState } from "react";
import { useParams } from "react-router";

function Buy() {
  const [buySummary, setBuySummary] = useState(null);
  const [sharesToBuy, setSharesToBuy] = useState(0)

  const { symbol } = useParams();

  useEffect(() => {
    axios.get("api/buy/summary", { params: { symbol }})
        .then(response => setBuySummary(response.data))
  }, []);
  
  const updateShareAmount = (value) => {
    var intValue = parseInt(value === "" ? 0 : value);

    if (isNaN(intValue)) {
        return;
    }
    setSharesToBuy(intValue);
  }

  return (
    <>
      <div>Buy ({symbol})</div>
      <div>
        <h3>Current $$$</h3>
        <p>$100,000.45</p>
      </div>
      <div>
        <h3>Transaction details</h3>
        <input type="text" placeholder="no. of shares" onChange={(e) => { updateShareAmount(e.target.value) }} value={sharesToBuy} />
        {buySummary && (
          <>
            <p>Current Price: ${buySummary.currentPrice}</p>
            <p>Max Shares: {buySummary.maxShareAmount}</p>
            <p>Total cost: ${sharesToBuy * buySummary.currentPrice}</p>
          </>
        )}
        <button>Submit</button>
      </div>
    </>
  );
}

export default Buy;
