document.addEventListener('DOMContentLoaded', function () {
    //URL parametrelerinden projectName'i al ve sayfada göster
    const params = new URLSearchParams(window.location.search);
    const projectName = params.get('project');
    
    if (projectName) {
        document.getElementById('project-name-container').innerHTML = `<h2>Project: ${decodeURIComponent(projectName)}</h2>`;
    }

    //Start CodeMirror Editors
    const sentPatternElement = document.getElementById('sent-pattern');
    const receivedPatternElement = document.getElementById('received-pattern');
    let sentPatternEditor, receivedPatternEditor;

    //Eğer 'sent-pattern' alanı varsa editör olarak başlat
    if (sentPatternElement) {
        sentPatternEditor = CodeMirror.fromTextArea(sentPatternElement, {
            lineNumbers: true,
            mode: 'javascript'// Varsayılan dil
        });
        sentPatternEditor.setSize(null, "800px");  // Yüksekliği 600px olarak ayarla
    }

    //Eğer 'received-pattern' alanı varsa editör olarak başlat
    if (receivedPatternElement) {
        receivedPatternEditor = CodeMirror.fromTextArea(receivedPatternElement, {
            lineNumbers: true,
            mode: 'javascript' //Varsayılan dil
        });
        receivedPatternEditor.setSize(null, "800px");  // Yüksekliği 600px olarak ayarla
    }

    //Kullanıcı dil seçimi yaptığında, editörlerin dil modlarını güncelle
    document.getElementById("sent-pattern-language").addEventListener("change", function () {
        var language = this.value;
        if (sentPatternEditor) {
            sentPatternEditor.setOption("mode", language);
        }
    });

    document.getElementById("received-pattern-language").addEventListener("change", function () {
        var language = this.value;
        if (receivedPatternEditor) {
            receivedPatternEditor.setOption("mode", language);
        }
    });

    // Home.html sayfasına özgü kodlar

    

    // Projects.html sayfasına özgü kodlar
    const projectList = document.getElementById('project-list');
    const filterInput = document.getElementById('filter');


    if (projectList && filterInput) {


        // Filter işlemi
        filterInput.addEventListener('keyup', () => {
            const filterValue = filterInput.value.toLowerCase();
            const projects = Array.from(projectList.getElementsByTagName('li'));
            projects.forEach(project => {
                const projectName = project.querySelector('strong').textContent.toLowerCase();
                project.style.display = projectName.includes(filterValue) ? '' : 'none';
            });
        });


    }



    // JSON Yönetim sayfasına özgü kodlar
    const jsonContainer = document.getElementById('json-container');
    if (jsonContainer) {

        const searchJsonInput = document.getElementById('search-json');


        searchJsonInput.addEventListener('input', () => {
            const searchValue = searchJsonInput.value.toLowerCase();
            renderJsonList(jsonDataList.filter(jsonData => jsonData.name.toLowerCase().includes(searchValue)));
        });
    }
});









