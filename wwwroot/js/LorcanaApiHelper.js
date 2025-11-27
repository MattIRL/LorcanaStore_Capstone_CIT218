// Reusable helper for interacting with Lorcana API in forms

/**
 * Populate form inputs with data from a CardApiModel
 * @param {Object} card - The card object from the API (CardApiModel)
 * @param {string} prefix - Optional prefix for input IDs (default: "")
 */
// Populate form inputs with data from a CardApiModel
export function selectCard(card, prefix = "") {
    // Fill standard text/number fields
    document.getElementById(prefix + "ID").value = card.unique_ID || "";
    document.getElementById(prefix + "CardName").value = card.name || "";
    document.getElementById(prefix + "Franchise").value = card.franchise || "";
    document.getElementById(prefix + "Image_URL").value = card.image || "";
    document.getElementById(prefix + "SetName").value = card.set_Name || "";
    document.getElementById(prefix + "Ink").value = card.cost ?? "";
    document.getElementById(prefix + "Willpower").value = card.willpower ?? "";
    document.getElementById(prefix + "Strength").value = card.strength ?? "";

    // Gem color mapping (string from API -> select option value)
    const gemMap = {
        "Amber": "0",
        "Amethyst": "1",
        "Emerald": "2",
        "Ruby": "3",
        "Sapphire": "4",
        "Steel": "5",
        "Amber, Amethyst": "6",
        "Amber, Emerald": "7",
        "Amber, Ruby": "8",
        "Amber, Sapphire": "9",
        "Amber, Steel": "10",
        "Amethyst, Emerald": "11",
        "Amethyst, Ruby": "12",
        "Amethyst, Sapphire": "13",
        "Amethyst, Steel": "14",
        "Emerald, Ruby": "15",
        "Emerald, Sapphire": "16",
        "Emerald, Steel": "17",
        "Ruby, Sapphire": "18",
        "Ruby, Steel": "19",
        "Sapphire, Steel": "20"
    };

    const selectElem = document.getElementById(prefix + "GemColor");
    if (selectElem) {
        selectElem.value = gemMap[card.color] || "";
    }

    // Hide the search results once a card is selected
    const results = document.getElementById(prefix + "searchResults");
    if (results) results.style.display = 'none';
}


/**
 * Attach search functionality to a search input and results container
 * @param {string} searchInputId - ID of the search input box
 * @param {string} searchBtnId - ID of the search button
 * @param {string} resultsContainerId - ID of the container to render results
 * @param {string} formPrefix - Optional prefix for inputs (default: "")
 */
export function attachCardSearch(searchInputId, searchBtnId, resultsContainerId, formPrefix = "") {
    const input = document.getElementById(searchInputId);
    const button = document.getElementById(searchBtnId);
    const container = document.getElementById(resultsContainerId);

    if (!input || !button || !container) return;

    button.addEventListener("click", async () => {
        const query = input.value.trim();
        if (!query) return;

        container.innerHTML = "<p>Searching...</p>";
        container.style.display = "block";

        try {
            const res = await fetch(`/Cards/Search?name=${encodeURIComponent(query)}`)
            if (!res.ok) throw new Error("API request failed");

            const data = await res.json();
            container.innerHTML = "";
            container.style.display = data.length ? "block" : "none";

            if (!data.length) {
                container.innerHTML = "<p>No results found.</p>";
                return;
            }

            data.forEach(card => {
                const div = document.createElement("div");
                div.classList.add("card-result");
                div.innerHTML = `
                    <img src="${card.image}" alt="${card.name}" style="width:200px; height:auto;">
                    <div class="card-overlay">${card.name}</div>
                `;
                div.addEventListener("click", () => selectCard(card, formPrefix));
                container.appendChild(div);
            });
        } catch (err) {
            container.innerHTML = "<p>Error fetching results.</p>";
            console.error(err);
        }
    });
}
