async function createParents(parents) {
    const txn = (await db).transaction("parent", "readwrite");
    const store = txn.store;

    const tasks = [];
    for (let i = 0; i < parents.length; i++) {
        const task = store.add(parents[i]);
        tasks.push(task);
    }

    await Promise.all([
        tasks,
        txn.done,
    ]);
}

async function getParents() {
    return (await db).transaction("parent").store.getAll();
}

async function exportDatabase(name) {
    const content = Module.FS.readFile(`/${name}`);
    const file = new File([content], name);
    const exportUrl = URL.createObjectURL(file);

    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = exportUrl;
    a.download = name;
    a.target = "_self";
    a.click();
}

async function deleteDatabase() {
    window.indexedDB.deleteDatabase("Database");
}
