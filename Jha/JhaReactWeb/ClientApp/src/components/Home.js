import { Tab } from 'bootstrap';
import '../Styles/Home.css';
import React, { useState, useEffect, useRef} from 'react';




export function Home() {
    const [statisticts, setStatistics] = useState({ totalTweets: 0 });
    const [refreshInterval, setRefreshInterval] = useState(1000);
    let actualRefreshInterval = useRef(1000);
    let timeoutRef = useRef(0);



    useEffect(() => {
        getLatestStatistics();
    }, []);



    async function getLatestStatistics() {
        const response = await fetch("https://localhost:7104/api/tweet/statistics");
        const newStatistics = await response.json();

        setStatistics(newStatistics);
        timeoutRef.current = setTimeout(getLatestStatistics, actualRefreshInterval.current);
    }


    function changeRefreshInterval(e) {
        setRefreshInterval(e.target.value);

        actualRefreshInterval.current = e.target.value;

        clearTimeout(timeoutRef.current);
        getLatestStatistics();
    }



    return (
        <section>
            <h1>Twitter Statistics</h1>
            <p>Created by <a href="https://www.linkedin.com/in/javan-joel/" target="_blank">Javan Joel</a> as a coding challenge for a Senor Staff Software Engineer position with JHA.</p>

            <div>
                Refresh Every &nbsp; 
                <select value={refreshInterval} onChange={changeRefreshInterval}>
                    <option value="250">1/4 second</option>
                    <option value="500">1/2 second</option>
                    <option value="1000">1 second</option>
                    <option value="5000">5 seconds</option>
                    <option value="10000">10 seconds</option>
                    <option value="15000">15 seconds</option>
                    <option value="30000">30 seconds</option>
                </select>
            </div>

            <h2>Total Tweets: <span>{statisticts.totalTweets}</span></h2>

            <div className="metrics">
                <div>
                    <h4>Top Hashtags</h4>
                    <ul>
                        {(Object.keys(statisticts.topHashtags || [])).map(key => 
                            <li>#{key} ({statisticts.topHashtags[key]})</li>
                        )}
                    </ul>
                </div>
                <div>
                    <h4>Top Mentions</h4>
                    <ul>
                        {(Object.keys(statisticts.topMentions || [])).map(key =>
                            <li>@{key} ({statisticts.topMentions[key]})</li>
                        )}
                    </ul>
                </div>
            </div>
        </section>
    );
}
