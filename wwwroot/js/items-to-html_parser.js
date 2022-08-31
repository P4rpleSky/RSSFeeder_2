const button = document.getElementById("close");
set_up_rss_feeder();

async function set_up_rss_feeder() {

    const form = document.getElementById("settings-switcher");
    await set_default_settings();
    await show_main_page();

    let timerId = setInterval(() => {
        show_main_page();
    }, 1000 * document.getElementById("update_time").value);

    form.addEventListener("submit", e => {
        e.preventDefault();
        clearTimeout(timerId);
        window.location.replace(button.href);
        show_main_page();
        timerId = setInterval(() => {
            show_main_page();
        }, 1000 * document.getElementById("update_time").value);
    });
}

async function set_default_settings() {
    const rssData = await fetch(`/get-rss-settings`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (rssData.ok === true) {
        const settings = await rssData.json();
        document.getElementById("rss_url").value = settings.RssUrl;
        document.getElementById("update_time").value = settings.UpdateTime;
    }
}

async function show_main_page() {

    const main = document.querySelector("div.main-block");
    main.innerHTML = ``;
    main.innerHTML = `
        <div class="container">
            <div class="row">
                <h2 id="loading">Loading...</h2>
            </div>
        </div>`;
    const response = await fetch("/put-items-json", {
        method: "PUT",
        headers: { "Accept": "application/json", "RssUrl": document.getElementById("rss_url").value }
    });
    if (response.ok === true) {
        const items = await response.json();
        main.innerHTML = ``;
        let view_switcher = document.getElementById("view_switcher");
        items.forEach(p => {

            let items_section = document.createElement("div");
            items_section.className += "container";
            let description = p.Description;
            if (!view_switcher.checked)
                description = description.replaceAll("<", "&lt").replaceAll(">", "&gt");

            items_section.innerHTML = `
                    <div class="row">
                        <a class="title" href="${p.Link}" target="_blank" rel="noopener noreferrer">${p.Title}</a>
                    </div>
                    <div class="decsription row">
                        <div class="s3">${description}</div>
                    </div>
                    <div class="row">
                        <div class="s3">${p.PubDate}</div>
                    </div>`;
            
            main.append(items_section)
        });
    }
}