import axios from "axios";
import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";

function Buy() {
  const [buySummary, setBuySummary] = useState(null);
  const [sharesToBuy, setSharesToBuy] = useState(0);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);

  const { symbol } = useParams();
  const navigate = useNavigate();

  useEffect(() => {
    axios.get("api/buy/summary", { params: { symbol }})
        .then(response => setBuySummary(response.data))
        .catch(error => setError("Failed to load buy summary. Please try again."));
  }, [symbol]);
  
  const updateShareAmount = (value) => {
    const intValue = parseInt(value === "" ? 0 : value);
    if (isNaN(intValue) || intValue < 0) {
      return;
    }
    setSharesToBuy(intValue);
  }

  const executeBuy = () => {
    if (sharesToBuy <= 0) {
      setError("Please enter a valid number of shares to buy.");
      return;
    }
    axios.post('api/buy/execute', {
      symbol,
      amount: sharesToBuy
    }).then(response => {
      setSuccess(true);
      setTimeout(() => navigate("/dashboard"), 2000);
    }).catch(error => {
      setError("Failed to execute buy order. Please try again.");
    });
  }

  const formatCurrency = new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
  });

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-8 text-center">Buy Order: {symbol}</h1>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="bg-white rounded-lg shadow-md p-6">
          <h2 className="text-xl font-semibold mb-4">Account Summary</h2>
          <div className="space-y-4">
            <div>
              <h3 className="text-lg font-medium text-gray-700">Available Cash</h3>
              <p className="text-2xl font-bold text-green-600">{buySummary ? formatCurrency.format(buySummary.walletAmount) : "Loading..."}</p>
            </div>
          </div>
        </div>
        <div className="bg-white rounded-lg shadow-md p-6">
          <h2 className="text-xl font-semibold mb-4">Transaction Details</h2>
          <div className="space-y-4">
            <div>
              <label htmlFor="shareAmount" className="block text-sm font-medium text-gray-700">Number of Shares</label>
              <input
                type="number"
                id="shareAmount"
                placeholder="Enter number of shares"
                onChange={(e) => updateShareAmount(e.target.value)}
                value={sharesToBuy}
                className="mt-1 block w-full px-3 py-2 bg-white border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
              />
            </div>
            {buySummary && (
              <>
                <div>
                  <h3 className="text-sm font-medium text-gray-700">Current Price</h3>
                  <p className="text-lg font-semibold">{formatCurrency.format(buySummary.currentPrice)}</p>
                </div>
                <div>
                  <h3 className="text-sm font-medium text-gray-700">Max Shares Available</h3>
                  <p className="text-lg font-semibold">{buySummary.maxShareAmount}</p>
                </div>
                <div>
                  <h3 className="text-sm font-medium text-gray-700">Total Cost</h3>
                  <p className="text-xl font-bold text-blue-600">{formatCurrency.format(sharesToBuy * buySummary.currentPrice)}</p>
                </div>
              </>
            )}
            {error && <p className="text-red-500 text-sm">{error}</p>}
            {success && <p className="text-green-500 text-sm">Buy order executed successfully! Redirecting to dashboard...</p>}
            <button
              onClick={executeBuy}
              className="w-full bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50"
            >
              Execute Buy Order
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Buy;
