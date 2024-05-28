import React, { useState } from "react";
import axios from "../common/AuthInterceptor";
import { useNavigate } from "react-router-dom";

function Trade() {
  const [searchQuery, setSearchQuery] = useState("");
  const [companies, setCompanies] = useState([]);

  const navigate = useNavigate();

  const search = (e) => {
    e.preventDefault();
    axios.get('api/company', { params: { query: searchQuery } })
      .then(response => setCompanies(response.data))
  }

  const navigateToBuy = (symbol) => {
    navigate(`/trade/buy/${symbol}`)
  }

  return (
    <div>
      <div>Trade</div>
      <div>Search for symbol =&#62; Buy or sell</div>
      <div>
        <h1>Search</h1>
        <form onSubmit={search}>
          <input type="text" placeholder="Enter company symbol" onChange={(e) => setSearchQuery(e.target.value)} value={searchQuery} />
          <button type="submit">Search</button>
        </form>
        <div>
          <ul>
            {companies.map(c => <li key={c.symbol} onClick={() => navigateToBuy(c.symbol)}>{c.name} ({c.symbol})</li>)}
          </ul>
        </div>
      </div>
      <div>Show available cash somewhere</div>
    </div>
  );
}

export default Trade;
