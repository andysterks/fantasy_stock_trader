import React from 'react'
import { useParams } from 'react-router'

function Buy() {
  const { symbol } = useParams();    
    
  return (
    <>
        <div>Buy ({symbol})</div>
        <div>
            <h3>Current $$$</h3>
            <p>$100,000.45</p>
        </div>
        <div>
            <h3>Transaction details</h3>
            <input type="text" placeholder='no. of shares' />
            <button>Submit</button>
        </div>
    </>
  )
}

export default Buy