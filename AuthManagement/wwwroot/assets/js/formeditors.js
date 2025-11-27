window.initQuillEditors = () => {

    // Snow Editor
    const snow = document.querySelector('#snow-editor');
    if (snow) {
        new Quill('#snow-editor', {
            theme: 'snow',
            modules: {
                syntax: true,
                toolbar: '#snow-toolbar'
            }
        });
    }

    // Bubble Editor
    const bubble = document.querySelector('#bubble-editor');
    if (bubble) {
        new Quill('#bubble-editor', {
            theme: 'bubble',
            modules: {
                syntax: true,
                toolbar: '#bubble-toolbar'
            }
        });
    }

    // Full Editor
    const full = document.querySelector('#full-editor');
    if (full) {
        new Quill('#full-editor', {
            theme: 'snow',
            modules: {
                syntax: true,
                toolbar: [
                    [{ 'font': [] }, { 'size': [] }],
                    ['bold', 'italic', 'underline', 'strike'],
                    [{ 'color': [] }, { 'background': [] }],
                    [{ 'script': 'sub' }, { 'script': 'super' }],
                    [{ 'header': 1 }, { 'header': 2 }, 'blockquote', 'code-block'],
                    [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                    ['link', 'image', 'video'],
                    ['clean']
                ]
            }
        });
    }
};
