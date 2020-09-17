
window.ytPlayer = {
    player: null,
    loadApi: function() {
        // This code loads the IFrame Player API code asynchronously.
        var tag = document.createElement('script');

        tag.src = "https://www.youtube.com/iframe_api";
        var firstScriptTag = document.getElementsByTagName('script')[0];
        firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
    },

    createPlayer: function (element) {
        if (this.player) {
            console.log('YouTube player is already initialized.');
            return;
        }

        this.loadApi();

        var self = this;
        window.onYouTubeIframeAPIReady = function() {
            self.player = new YT.Player(element,
                {
                    videoId: '',
                    events: {

                    },
                    playerVars: {
                        controls: 0,
                        disablekb: 1,
                        enablejsapi: 1
                    }
                });
        }
    }
}